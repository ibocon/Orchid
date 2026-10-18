#if ANDROID
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Orchid.Core.Domain.Common;
using Orchid.Core.Sync;
using Orchid.Core.Sync.Relay;

namespace Orchid.Mobile.Platforms.Android;

/// <summary>
/// 허브 연결과 코디네이터를 소유하는 Foreground Service입니다.
/// </summary>
[global::Android.App.Service(ForegroundServiceType = ForegroundService.TypeDataSync, Exported = false)]
public sealed class RelayForegroundService : global::Android.App.Service
{
    /// <summary>
    /// 전달할 페어링 키 문자열의 Intent extra 이름입니다.
    /// </summary>
    public const string ExtraPairingPayload = "pairingPayload";

    private const int NotificationId = 1001;
    private const string ChannelId = "orchid-relay";

    /// <inheritdoc />
    public override IBinder? OnBind(Intent? intent)
    {
        return null;
    }

    /// <inheritdoc />
    public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
    {
        if (OperatingSystem.IsAndroidVersionAtLeast(29))
        {
            StartForeground(NotificationId, BuildNotification(), ForegroundService.TypeDataSync);
        }
        else
        {
            StartForeground(NotificationId, BuildNotification());
        }

        Result<PairingKey> parsed = PairingKey.Parse(intent?.GetStringExtra(ExtraPairingPayload));
        if (parsed.IsFailure)
        {
            StopSelf();
            return StartCommandResult.NotSticky;
        }

        ClipboardSyncCoordinator coordinator = IPlatformApplication.Current!.Services.GetRequiredService<ClipboardSyncCoordinator>();
        _ = StartCoordinatorAsync(coordinator, parsed.Value);
        return StartCommandResult.Sticky;
    }

    private static async Task StartCoordinatorAsync(ClipboardSyncCoordinator coordinator, PairingKey key)
    {
        try
        {
            await
                coordinator.StartAsync(key, Guid.NewGuid())
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            global::Android.Util.Log.Error("Orchid", $"Relay start failed: {exception}");
        }
    }

    private Notification BuildNotification()
    {
        // minSdk 23은 알림 채널(26 이상)이 없으므로 채널 없는 Builder로 대체한다.
        if (!OperatingSystem.IsAndroidVersionAtLeast(26))
        {
#pragma warning disable CS0618 // 채널이 없는 구형 API에서만 사용한다.
            return new Notification.Builder(this)
                .SetContentTitle("Orchid 클립보드 동기화 중")
                .SetSmallIcon(global::Android.Resource.Drawable.IcDialogInfo)
                .Build()!;
#pragma warning restore CS0618
        }

        NotificationManager manager = (NotificationManager)GetSystemService(NotificationService)!;
        manager.CreateNotificationChannel(new NotificationChannel(ChannelId, "Orchid Sync", NotificationImportance.Low));
        return new Notification.Builder(this, ChannelId)
            .SetContentTitle("Orchid 클립보드 동기화 중")
            .SetSmallIcon(global::Android.Resource.Drawable.IcDialogInfo)
            .Build()!;
    }
}
#endif
