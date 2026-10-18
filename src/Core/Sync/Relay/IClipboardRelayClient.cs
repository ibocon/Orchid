using Orchid.Contract.Payload;

namespace Orchid.Core.Sync.Relay;

/// <summary>
/// 릴레이 서버와 암호화 페이로드를 주고받는 전송 계약입니다.
/// </summary>
public interface IClipboardRelayClient : IAsyncDisposable
{
    /// <summary>
    /// 원격 페이로드 수신 시 발생합니다.
    /// </summary>
    event Func<EncryptedPayload, Task>? PayloadReceived;

    /// <summary>
    /// 허브에 연결하고 페어링 그룹에 가입합니다.
    /// </summary>
    /// <param name="pairingId">페어링 식별자</param>
    /// <param name="cancellationToken">취소 토큰</param>
    /// <returns>비동기 작업</returns>
    Task ConnectAsync(string pairingId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 페이로드를 같은 페어링의 다른 디바이스로 보냅니다.
    /// </summary>
    /// <param name="pairingId">페어링 식별자</param>
    /// <param name="payload">암호화된 페이로드</param>
    /// <param name="cancellationToken">취소 토큰</param>
    /// <returns>비동기 작업</returns>
    Task SendAsync(string pairingId, EncryptedPayload payload, CancellationToken cancellationToken = default);
}
