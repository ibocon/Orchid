#if IOS
using Foundation;
using Orchid.Core.Abstraction;
using UIKit;

namespace Orchid.Mobile.Platforms.iOS;

/// <summary>
/// Apple iOS UIPasteboard 기반의 시스템 클립보드 어댑터 구현체입니다.
/// </summary>
public sealed class IosClipboardAdapter : ISystemClipboard
{
    /// <inheritdoc />
    public Task<string?> GetTextAsync()
    {
        string? text = UIPasteboard.General.String;
        return Task.FromResult<string?>(text);
    }

    /// <inheritdoc />
    public Task SetTextAsync(string text)
    {
        UIPasteboard.General.String = text;
        return Task.CompletedTask;
    }
}
#endif
