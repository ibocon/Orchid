using Orchid.Contract.Model;
using Orchid.Contract.Payload;

namespace Orchid.Contract.Request;

/// <summary>
/// 새 클립보드 암호화 블롭 업로드 요청 계약입니다.
/// </summary>
/// <param name="DeviceId">발신 디바이스 식별자</param>
/// <param name="Platform">발신 디바이스 플랫폼</param>
/// <param name="ContentType">클립보드 데이터 콘텐츠 유형</param>
/// <param name="Payload">E2EE 암호화된 페이로드 데이터</param>
public sealed record PushClipboardRequest(
    Guid DeviceId,
    DevicePlatform Platform,
    ContentType ContentType,
    EncryptedPayload Payload);
