using MediatR;
using Orchid.Contract.Payload;

namespace Orchid.Service.Clipboard.Application.Command;

/// <summary>
/// 클립보드 암호화 데이터를 수신하여 실시간 브로드캐스트하는 CQRS 커맨드입니다.
/// </summary>
/// <param name="DeviceId">발신 디바이스 식별자</param>
/// <param name="Payload">암호화된 E2EE 페이로드</param>
public sealed record PushClipboardCommand(
    Guid DeviceId,
    EncryptedPayload Payload) : IRequest<bool>;
