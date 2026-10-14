using Orchid.Contract.Payload;

namespace Orchid.Service.Clipboard.Application.Common;

/// <summary>
/// 연결된 클라이언트들로의 실시간 클립보드 브로드캐스트를 추상화하는 서비스 인터페이스입니다.
/// </summary>
public interface IRealtimeBroadcastService
{
    /// <summary>
    /// 암호화된 클립보드 페이로드를 연결된 클라이언트들로 실시간 브로드캐스트합니다.
    /// </summary>
    /// <param name="payload">암호화된 페이로드</param>
    /// <param name="cancellationToken">취소 토큰</param>
    /// <returns>비동기 작업 태스크</returns>
    Task BroadcastAsync(EncryptedPayload payload, CancellationToken cancellationToken = default);
}
