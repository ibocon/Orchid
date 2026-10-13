using Microsoft.JSInterop;
using Orchid.Core.Abstraction;

namespace Orchid.Web.Service;

/// <summary>
/// 브라우저 navigator.clipboard JS API와 상호작용하는 웹 클립보드 어댑터 구현체입니다.
/// </summary>
public sealed class WebClipboardAdapter : ISystemClipboard
{
    private readonly IJSRuntime _js;

    /// <summary>
    /// <see cref="WebClipboardAdapter"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="js">JavaScript 런타임 인스턴스</param>
    public WebClipboardAdapter(IJSRuntime js) => _js = js ?? throw new ArgumentNullException(nameof(js));

    /// <inheritdoc />
    public async Task<string?> GetTextAsync()
    {
        try
        {
            return await _js.InvokeAsync<string?>("navigator.clipboard.readText").ConfigureAwait(false);
        }
        catch (JSException)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public async Task SetTextAsync(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        await _js.InvokeVoidAsync("navigator.clipboard.writeText", text).ConfigureAwait(false);
    }
}
