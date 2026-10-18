namespace Orchid.Core.Sync.Relay;

/// <summary>
/// 클립보드 릴레이 허브 접속 옵션입니다.
/// </summary>
/// <param name="HubUri"><c>/hubs/clipboard</c>까지 포함한 절대 URI</param>
public sealed record RelayOptions(Uri HubUri);
