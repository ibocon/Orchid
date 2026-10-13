using Microsoft.AspNetCore.SignalR;
using Orchid.Contract.Payload;

namespace Orchid.Service.Clipboard.Infrastructure.Realtime;

/// <summary>
/// 멀티 디바이스 간 실시간 E2EE 클립보드 스트리밍 및 동기화를 중계하는 SignalR 허브입니다.
/// </summary>
public sealed class ClipboardHub : Hub
{
    /// <summary>
    /// 클라이언트가 E2EE 암호화된 페이로드를 허브로 전송할 때 호출됩니다.
    /// </summary>
    /// <param name="payload">암호화된 클립보드 페이로드</param>
    public async Task SendClipboard(EncryptedPayload payload)
    {
        // 발신자를 제외한 동일 계정의 다른 연결된 디바이스로 브로드캐스트
        await Clients.Others.SendAsync("OnClipboardReceived", payload).ConfigureAwait(false);
    }
}
