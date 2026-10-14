using Microsoft.AspNetCore.SignalR;
using Orchid.Contract.Payload;
using Orchid.Service.Clipboard.Application.Common;

namespace Orchid.Service.Clipboard.Infrastructure.Realtime;

/// <summary>
/// SignalR 허브 컨텍스트를 활용하여 클립보드 페이로드를 브로드캐스트하는 인프라 서비스 구현체입니다.
/// </summary>
public sealed class SignalRBroadcastService : IRealtimeBroadcastService
{
    private readonly IHubContext<ClipboardHub> _hubContext;

    /// <summary>
    /// <see cref="SignalRBroadcastService"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="hubContext">SignalR 허브 컨텍스트</param>
    public SignalRBroadcastService(IHubContext<ClipboardHub> hubContext)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    /// <inheritdoc />
    public async Task BroadcastAsync(EncryptedPayload payload, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(payload);

        await _hubContext.Clients.All.SendAsync("OnClipboardReceived", payload, cancellationToken).ConfigureAwait(false);
    }
}
