using System.Diagnostics.CodeAnalysis;
using Orchid.Contract.Model;

namespace Orchid.Service.Clipboard.Domain.Entity;

/// <summary>
/// 서버 측에 저장되는 암호화된 클립보드 블롭 불변 엔티티입니다. 서버는 평문 내용을 전혀 알 수 없습니다(Zero-Knowledge).
/// </summary>
[SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Cryptographic binary payload required for database storage")]
public sealed class ClipboardBlob
{
    /// <summary>
    /// <see cref="ClipboardBlob"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public ClipboardBlob(
        Guid id,
        Guid sourceDeviceId,
        DevicePlatform platform,
        ContentType contentType,
        byte[] cipherText,
        byte[] nonce,
        byte[] tag,
        string algorithm,
        DateTimeOffset createdAtUtc)
    {
        Id = id;
        SourceDeviceId = sourceDeviceId;
        Platform = platform;
        ContentType = contentType;
        CipherText = cipherText;
        Nonce = nonce;
        Tag = tag;
        Algorithm = algorithm;
        CreatedAtUtc = createdAtUtc;
    }

    /// <summary>
    /// 블롭 고유 식별자
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 발신 디바이스 식별자
    /// </summary>
    public Guid SourceDeviceId { get; init; }

    /// <summary>
    /// 발신 플랫폼
    /// </summary>
    public DevicePlatform Platform { get; init; }

    /// <summary>
    /// 콘텐츠 유형
    /// </summary>
    public ContentType ContentType { get; init; }

    /// <summary>
    /// 암호화된 바이트 배열
    /// </summary>
    public byte[] CipherText { get; init; }

    /// <summary>
    /// 암호화에 사용된 논스(12바이트)
    /// </summary>
    public byte[] Nonce { get; init; }

    /// <summary>
    /// 인증 태그(16바이트)
    /// </summary>
    public byte[] Tag { get; init; }

    /// <summary>
    /// 사용된 암호화 알고리즘
    /// </summary>
    public string Algorithm { get; init; }

    /// <summary>
    /// 생성 일시 (UTC)
    /// </summary>
    public DateTimeOffset CreatedAtUtc { get; init; }
}
