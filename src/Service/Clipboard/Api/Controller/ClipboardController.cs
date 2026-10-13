using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orchid.Contract.Request;
using Orchid.Service.Clipboard.Application.Command;

namespace Orchid.Service.Clipboard.Api.Controller;

/// <summary>
/// E2EE 암호화 클립보드 블롭 업로드 및 동기화를 담당하는 REST API 컨트롤러입니다.
/// </summary>
[ApiController]
[Route("api")]
public sealed class ClipboardController : ControllerBase
{
    private readonly ISender _sender;

    /// <summary>
    /// <see cref="ClipboardController"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="sender">MediatR ISender 인스턴스</param>
    public ClipboardController(ISender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    /// <summary>
    /// 새로운 E2EE 암호화 클립보드 페이로드를 업로드하고 실시간 동기화를 트리거합니다.
    /// </summary>
    /// <param name="request">클립보드 푸시 요청 DTO</param>
    /// <param name="cancellationToken">취소 토큰</param>
    /// <returns>처리 성공 여부</returns>
    [HttpPost("clipboard/push")]
    public async Task<IActionResult> Push([FromBody] PushClipboardRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        PushClipboardCommand command = new(request.DeviceId, request.Payload);
        bool success = await _sender.Send(command, cancellationToken).ConfigureAwait(false);

        return success ? Ok(new { success = true }) : BadRequest();
    }
}
