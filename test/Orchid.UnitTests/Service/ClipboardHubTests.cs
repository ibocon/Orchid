using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Orchid.Contract.Payload;
using Orchid.Service.Clipboard.Infrastructure.Realtime;
using Xunit;

namespace Orchid.UnitTests.Service;

/// <summary>
/// <see cref="ClipboardHub"/>의 페어링 그룹 격리 중계를 검증하는 단위 테스트입니다.
/// </summary>
public sealed class ClipboardHubTests
{
    private static readonly EncryptedPayload Payload = new(Guid.NewGuid(), [1], [2], [3], "AES-256-GCM", DateTimeOffset.UtcNow);

    [Fact]
    public async Task JoinPairing_When_PairingId_Given_Should_Add_Connection_To_Group()
    {
        // Given
        var groups = new Mock<IGroupManager>();
        ClipboardHub sut = CreateHub(groups.Object, new Mock<IHubCallerClients>().Object);

        // When
        await sut.JoinPairing("ABCDEF");

        // Then
        groups.Verify(g => g.AddToGroupAsync("conn-1", "ABCDEF", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SendClipboard_When_Payload_Given_Should_Relay_Only_To_Others_In_Group()
    {
        // Given
        var groupProxy = new Mock<IClientProxy>();
        var clients = new Mock<IHubCallerClients>();
        clients.Setup(c => c.OthersInGroup("ABCDEF")).Returns(groupProxy.Object);
        ClipboardHub sut = CreateHub(new Mock<IGroupManager>().Object, clients.Object);

        // When
        await sut.SendClipboard("ABCDEF", Payload);

        // Then
        groupProxy.Verify(
            p => p.SendCoreAsync("OnClipboardReceived", It.Is<object?[]>(a => ReferenceEquals(a[0], Payload)), It.IsAny<CancellationToken>()),
            Times.Once);
        clients.Verify(c => c.Others, Times.Never);
        clients.Verify(c => c.All, Times.Never);
    }

    [Fact]
    public async Task SendClipboard_When_PairingId_Blank_Should_Throw()
    {
        // Given
        ClipboardHub sut = CreateHub(new Mock<IGroupManager>().Object, new Mock<IHubCallerClients>().Object);

        // When
        Func<Task> act = () => sut.SendClipboard(" ", Payload);

        // Then
        await act.Should().ThrowAsync<ArgumentException>();
    }

    private static ClipboardHub CreateHub(IGroupManager groups, IHubCallerClients clients)
    {
        var context = new Mock<HubCallerContext>();
        context.SetupGet(c => c.ConnectionId).Returns("conn-1");
        return new ClipboardHub
        {
            Groups = groups,
            Clients = clients,
            Context = context.Object
        };
    }
}
