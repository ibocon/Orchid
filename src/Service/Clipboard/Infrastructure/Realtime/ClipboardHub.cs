using Microsoft.AspNetCore.SignalR;
using Orchid.Contract.Payload;

namespace Orchid.Service.Clipboard.Infrastructure.Realtime;

/// <summary>
/// 멀티 디바이스 간 실시간 E2EE 클립보드 스트리밍 및 동기화를 중계하는 SignalR 허브입니다.
/// </summary>
public sealed class ClipboardHub : Hub
{
    /// <summary>
    /// 연결을 페어링 그룹에 가입시킵니다.
    /// </summary>
    /// <param name="pairingId">페어링 키의 SHA-256 해시 16진 문자열</param>
    public async Task JoinPairing(string pairingId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pairingId);
        await
            Groups.AddToGroupAsync(Context.ConnectionId, pairingId)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// 같은 페어링 그룹의 다른 디바이스로 E2EE 암호화된 페이로드를 중계합니다.
    /// </summary>
    /// <param name="pairingId">대상 페어링 그룹</param>
    /// <param name="payload">암호화된 클립보드 페이로드</param>
    public async Task SendClipboard(string pairingId, EncryptedPayload payload)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pairingId);
        ArgumentNullException.ThrowIfNull(payload);
        await
            Clients.OthersInGroup(pairingId).SendAsync("OnClipboardReceived", payload)
            .ConfigureAwait(false);
    }
}
