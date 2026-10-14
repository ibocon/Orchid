namespace Orchid.Mobile.Service;

/// <summary>
/// 모바일 백그라운드 수명주기 및 주기적 동기화 워커 계약입니다.
/// </summary>
public interface IMobileSyncBackgroundWorker
{
    /// <summary>
    /// 백그라운드 워커 활성 여부
    /// </summary>
    bool IsActive { get; }
}
