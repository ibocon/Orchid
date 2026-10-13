using MediatR;
using Microsoft.AspNetCore.SignalR;
using Orchid.Service.Clipboard.Application.Command;
using Orchid.Service.Clipboard.Infrastructure.Realtime;

namespace Orchid.Service.Clipboard.Application.Handler;

/// <summary>
/// <see cref="PushClipboardCommand"/> 커맨드를 처리하여 연결된 클라이언트들로 SignalR 브로드캐스트를 수행하는 핸들러입니다.
/// </summary>
public sealed class PushClipboardCommandHandler : IRequestHandler<PushClipboardCommand, bool>
{
    private readonly IHubContext<ClipboardHub> _hubContext;

    /// <summary>
    /// <see cref="PushClipboardCommandHandler"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="hubContext">SignalR 클립보드 허브 컨텍스트</param>
    public PushClipboardCommandHandler(IHubContext<ClipboardHub> hubContext)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    /// <inheritdoc />
    public async Task<bool> Handle(PushClipboardCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // 다른 디바이스들로 실시간 E2EE 암호화 페이로드 브로드캐스트
        await _hubContext.Clients.All.SendAsync("OnClipboardReceived", request.Payload, cancellationToken).ConfigureAwait(false);
        return true;
    }
}
