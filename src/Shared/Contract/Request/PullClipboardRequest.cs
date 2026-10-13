namespace Orchid.Contract.Request;

/// <summary>
/// 증분(Delta) 클립보드 동기화 요청 계약입니다.
/// </summary>
/// <param name="DeviceId">요청 디바이스 식별자</param>
/// <param name="SinceTimestampUtc">동기화 기준 시점 (이 시간 이후 변경사항 요청)</param>
/// <param name="Limit">가져올 최대 항목 수</param>
public sealed record PullClipboardRequest(
    Guid DeviceId,
    DateTimeOffset? SinceTimestampUtc,
    int Limit = 50);
