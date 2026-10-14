using FluentAssertions;
using Microsoft.JSInterop;
using Moq;
using Orchid.Web.Service;
using Xunit;

namespace Orchid.UnitTests.Web;

/// <summary>
/// <see cref="WebClipboardAdapter"/>의 JavaScript interop 호출 및 브라우저 예외 격리를 검증하는 단위 테스트입니다.
/// </summary>
public sealed class WebClipboardAdapterTests
{
    [Fact]
    public async Task GetTextAsync_WhenJsThrows_ShouldReturnNull()
    {
        // Given
        var jsMock = new Mock<IJSRuntime>();
        jsMock.Setup(js => js.InvokeAsync<string?>(
            "navigator.clipboard.readText",
            It.IsAny<object?[]>()))
            .ThrowsAsync(new JSException("Clipboard permission denied"));

        var adapter = new WebClipboardAdapter(jsMock.Object);

        // When
        var result = await adapter.GetTextAsync();

        // Then
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTextAsync_WhenSuccessful_ShouldReturnText()
    {
        // Given
        var jsMock = new Mock<IJSRuntime>();
        jsMock.Setup(js => js.InvokeAsync<string?>(
            "navigator.clipboard.readText",
            It.IsAny<object?[]>()))
            .ReturnsAsync("Hello Web");

        var adapter = new WebClipboardAdapter(jsMock.Object);

        // When
        var result = await adapter.GetTextAsync();

        // Then
        result.Should().Be("Hello Web");
    }

    [Fact]
    public async Task SetTextAsync_WhenNullText_ShouldThrowArgumentNullException()
    {
        // Given
        var jsMock = new Mock<IJSRuntime>();
        var adapter = new WebClipboardAdapter(jsMock.Object);

        // When & Then
        Func<Task> act = async () => await adapter.SetTextAsync(null!);
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
