#if ANDROID
using Android.App;
using Android.Content.PM;
using Orchid.Core.Notification;

namespace Orchid.Mobile.Platforms.Android;

/// <summary>
/// Orchid Android 메인 액티비티 진입점입니다.
/// </summary>
[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
public class MainActivity : MauiAppCompatActivity
{
    private const int NotificationPermissionRequestCode = 1002;
    private const string RequestedPreferenceKey = "notification-permission-requested";

    /// <inheritdoc />
    protected override void OnCreate(global::Android.OS.Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (!OperatingSystem.IsAndroidVersionAtLeast(33))
        {
            return;
        }

        bool granted = CheckSelfPermission(global::Android.Manifest.Permission.PostNotifications) == Permission.Granted;
        bool requestedBefore = Preferences.Default.Get(RequestedPreferenceKey, false);

        if (NotificationPermissionPolicy.ShouldRequest(granted, requestedBefore))
        {
            Preferences.Default.Set(RequestedPreferenceKey, true);
            RequestPermissions([global::Android.Manifest.Permission.PostNotifications], NotificationPermissionRequestCode);
        }
    }
}
#endif
