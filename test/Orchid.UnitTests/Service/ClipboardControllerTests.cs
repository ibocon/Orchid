using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Orchid.Contract.Model;
using Orchid.Contract.Payload;
using Orchid.Contract.Request;
using Orchid.Service.Clipboard.Api.Controller;
using Orchid.Service.Clipboard.Application.Command;
using Xunit;

namespace Orchid.UnitTests.Service;

/// <summary>
/// <see cref="ClipboardController"/>의 HTTP 응답 매핑(200 Ok 및 400 BadRequest) 동작을 검증하는 단위 테스트입니다.
/// </summary>
public sealed class ClipboardControllerTests
{
    [Fact]
    public async Task Push_When_Sender_Succeeds_Should_Return_Ok()
    {
        // Given
        var senderMock = new Mock<ISender>();
        senderMock.Setup(s => s.Send(It.IsAny<PushClipboardCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var controller = new ClipboardController(senderMock.Object);
        var payload = new EncryptedPayload(Guid.NewGuid(), [1], [2], [3], "AES", DateTimeOffset.UtcNow);
        var request = new PushClipboardRequest(Guid.NewGuid(), DevicePlatform.Windows, ContentType.Text, payload);

        // When
        var response = await controller.Push(request, CancellationToken.None);

        // Then
        response.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Push_When_Sender_Fails_Should_Return_BadRequest()
    {
        // Given
        var senderMock = new Mock<ISender>();
        senderMock.Setup(s => s.Send(It.IsAny<PushClipboardCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var controller = new ClipboardController(senderMock.Object);
        var payload = new EncryptedPayload(Guid.NewGuid(), [1], [2], [3], "AES", DateTimeOffset.UtcNow);
        var request = new PushClipboardRequest(Guid.NewGuid(), DevicePlatform.Windows, ContentType.Text, payload);

        // When
        var response = await controller.Push(request, CancellationToken.None);

        // Then
        response.Should().BeOfType<BadRequestResult>();
    }
}
