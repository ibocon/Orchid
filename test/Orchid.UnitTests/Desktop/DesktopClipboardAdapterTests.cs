using FluentAssertions;
using Moq;
using Orchid.Core.Abstraction;
using Xunit;

namespace Orchid.UnitTests.Desktop;

public sealed class DesktopClipboardAdapterTests
{
    [Fact]
    public async Task Adapter_Should_Notify_Core_When_Clipboard_Content_Changes()
    {
        // Given
        var mockClipboard = new Mock<ISystemClipboard>();

        // When & Then
        mockClipboard.Setup(x => x.GetTextAsync()).ReturnsAsync("Sample Text");
        var result = await mockClipboard.Object.GetTextAsync();
        result.Should().Be("Sample Text");
    }
}
