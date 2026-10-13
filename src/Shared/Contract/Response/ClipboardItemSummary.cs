using Orchid.Contract.Model;

namespace Orchid.Contract.Response;

/// <summary>
/// 원문 평문이 배제된 클립보드 항목 메타데이터 요약 응답 계약입니다.
/// </summary>
/// <param name="Id">항목 고유 식별자</param>
/// <param name="SourceDeviceId">원본 발신 디바이스 식별자</param>
/// <param name="Platform">원본 발신 플랫폼</param>
/// <param name="ContentType">콘텐츠 유형</param>
/// <param name="SizeBytes">암호화 페이로드 크기(바이트 단위)</param>
/// <param name="CreatedAtUtc">생성 일시 (UTC)</param>
public sealed record ClipboardItemSummary(
    Guid Id,
    Guid SourceDeviceId,
    DevicePlatform Platform,
    ContentType ContentType,
    long SizeBytes,
    DateTimeOffset CreatedAtUtc);
