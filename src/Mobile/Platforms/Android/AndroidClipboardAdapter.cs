#if ANDROID
using Android.Content;
using Orchid.Core.Abstraction;

namespace Orchid.Mobile.Platforms.Android;

/// <summary>
/// Android 네이티브 ClipboardManager를 활용한 시스템 클립보드 어댑터 구현체입니다.
/// </summary>
public sealed class AndroidClipboardAdapter : ISystemClipboard
{
    private readonly Context _context;

    /// <summary>
    /// <see cref="AndroidClipboardAdapter"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="context">Android 애플리케이션 컨텍스트</param>
    public AndroidClipboardAdapter(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public Task<string?> GetTextAsync()
    {
        ClipboardManager? clipboard = (ClipboardManager?)_context.GetSystemService(Context.ClipboardService);
        if (clipboard is not null && clipboard.HasPrimaryClip && clipboard.PrimaryClip?.ItemCount > 0)
        {
            string? text = clipboard.PrimaryClip.GetItemAt(0)?.Text;
            return Task.FromResult(text);
        }

        return Task.FromResult<string?>(null);
    }

    /// <inheritdoc />
    public Task SetTextAsync(string text)
    {
        ClipboardManager? clipboard = (ClipboardManager?)_context.GetSystemService(Context.ClipboardService);
        if (clipboard is not null)
        {
            ClipData clip = ClipData.NewPlainText("Orchid", text);
            clipboard.PrimaryClip = clip;
        }

        return Task.CompletedTask;
    }
}
#endif
