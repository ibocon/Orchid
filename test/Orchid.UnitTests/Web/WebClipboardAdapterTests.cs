using FluentAssertions;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using Moq;
using Orchid.Web.Service;
using Xunit;

namespace Orchid.UnitTests.Web;

public sealed class WebClipboardAdapterTests
{
    [Fact]
    public async Task GetTextAsync_WhenJsThrows_ShouldReturnNull()
    {
        // Given
        var jsMock = new Mock<IJSRuntime>();
        jsMock.Setup(x => x.InvokeAsync<string?>(
                "navigator.clipboard.readText",
                It.IsAny<object[]>()))
            .ThrowsAsync(new JSException("Permission denied"));

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
        jsMock.Setup(x => x.InvokeAsync<string?>(
                "navigator.clipboard.readText",
                It.IsAny<object[]>()))
            .ReturnsAsync("Copied Content");

        var adapter = new WebClipboardAdapter(jsMock.Object);

        // When
        var result = await adapter.GetTextAsync();

        // Then
        result.Should().Be("Copied Content");
    }

    [Fact]
    public async Task SetTextAsync_WhenNullText_ShouldThrowArgumentNullException()
    {
        // Given
        var jsMock = new Mock<IJSRuntime>();
        var adapter = new WebClipboardAdapter(jsMock.Object);

        // When & Then
        var act = async () => await adapter.SetTextAsync(null!);
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
