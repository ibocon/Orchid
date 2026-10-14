using System.Diagnostics.CodeAnalysis;

namespace Orchid.Contract.Payload;

/// <summary>
/// 종단간 암호화(E2EE) 처리된 클립보드 페이로드 계약입니다.
/// </summary>
/// <param name="Id">페이로드 고유 식별자</param>
/// <param name="CipherText">AES-GCM 암호화된 바이트 배열</param>
/// <param name="Nonce">암호화에 사용된 고유 논스(IV, 12바이트)</param>
/// <param name="Tag">인증 태그(16바이트)</param>
/// <param name="Algorithm">암호화 알고리즘 식별자 (예: AES-256-GCM)</param>
/// <param name="CreatedAtUtc">생성 일시 (UTC)</param>
[SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Cryptographic payload DTO requires byte arrays")]
public sealed record EncryptedPayload(
    Guid Id,
    byte[] CipherText,
    byte[] Nonce,
    byte[] Tag,
    string Algorithm,
    DateTimeOffset CreatedAtUtc);
