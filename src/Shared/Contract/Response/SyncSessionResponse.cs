using Orchid.Contract.Model;

namespace Orchid.Contract.Response;

/// <summary>
/// 실시간 WebSocket/SignalR 연결 세션 정보 응답 계약입니다.
/// </summary>
/// <param name="SessionId">세션 고유 식별자</param>
/// <param name="DeviceId">인증된 디바이스 식별자</param>
/// <param name="Platform">인증된 디바이스 플랫폼</param>
/// <param name="ConnectedAtUtc">연결 수립 일시 (UTC)</param>
/// <param name="IsActive">세션 활성 여부</param>
public sealed record SyncSessionResponse(
    Guid SessionId,
    Guid DeviceId,
    DevicePlatform Platform,
    DateTimeOffset ConnectedAtUtc,
    bool IsActive);
