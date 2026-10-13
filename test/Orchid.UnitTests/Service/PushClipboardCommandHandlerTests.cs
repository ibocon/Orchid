using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Orchid.Contract.Payload;
using Orchid.Service.Clipboard.Application.Command;
using Orchid.Service.Clipboard.Application.Handler;
using Orchid.Service.Clipboard.Infrastructure.Realtime;
using Xunit;

namespace Orchid.UnitTests.Service;

public sealed class PushClipboardCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Broadcast_EncryptedPayload_To_Connected_Clients()
    {
        // Given
        var hubMock = new Mock<IHubContext<ClipboardHub>>();
        var clientsMock = new Mock<IHubClients>();
        var clientProxyMock = new Mock<IClientProxy>();

        hubMock.Setup(h => h.Clients).Returns(clientsMock.Object);
        clientsMock.Setup(c => c.All).Returns(clientProxyMock.Object);

        var handler = new PushClipboardCommandHandler(hubMock.Object);
        var payload = new EncryptedPayload(Guid.NewGuid(), [1, 2], [3], [4], "AES-256-GCM", DateTimeOffset.UtcNow);
        var command = new PushClipboardCommand(Guid.NewGuid(), payload);

        // When
        var result = await handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().BeTrue();
        clientProxyMock.Verify(
            x => x.SendCoreAsync(
                "OnClipboardReceived",
                It.Is<object?[]>(args => args.Length == 1 && ReferenceEquals(args[0], payload)),
                default),
            Times.Once);
    }
}
