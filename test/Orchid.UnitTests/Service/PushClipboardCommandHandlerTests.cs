using FluentAssertions;
using Moq;
using Orchid.Contract.Payload;
using Orchid.Service.Clipboard.Application.Command;
using Orchid.Service.Clipboard.Application.Common;
using Orchid.Service.Clipboard.Application.Handler;
using Xunit;

namespace Orchid.UnitTests.Service;

public sealed class PushClipboardCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Broadcast_EncryptedPayload_To_Connected_Clients()
    {
        // Given
        var broadcastMock = new Mock<IRealtimeBroadcastService>();
        var handler = new PushClipboardCommandHandler(broadcastMock.Object);
        var payload = new EncryptedPayload(Guid.NewGuid(), [1, 2], [3], [4], "AES-256-GCM", DateTimeOffset.UtcNow);
        var command = new PushClipboardCommand(Guid.NewGuid(), payload);

        // When
        var result = await handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().BeTrue();
        broadcastMock.Verify(
            x => x.BroadcastAsync(payload, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
