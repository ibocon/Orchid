using System.Diagnostics.CodeAnalysis;

namespace Orchid.Contract.Payload;

/// <summary>
/// 디바이스 간 E2EE 키 교환을 위한 ECDH 공개키 페이로드 계약입니다.
/// </summary>
/// <param name="DeviceId">요청 디바이스 고유 식별자</param>
/// <param name="PublicKey">디바이스 공개키 바이트 배열</param>
/// <param name="KeyAlgorithm">키 교환 알고리즘 식별자 (예: ECDH-P256, X25519)</param>
/// <param name="TimestampUtc">요청 생성 일시 (UTC)</param>
[SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Cryptographic payload DTO requires byte arrays")]
public sealed record KeyExchangePayload(
    Guid DeviceId,
    byte[] PublicKey,
    string KeyAlgorithm,
    DateTimeOffset TimestampUtc);
