namespace Orchid.Mobile.Service;

/// <summary>
/// 모바일 앱의 라이프사이클에 맞추어 동작하는 백그라운드 동기화 워커 구현체입니다.
/// </summary>
public sealed class MobileSyncBackgroundWorker : IMobileSyncBackgroundWorker
{
    private bool _isActive;

    /// <inheritdoc />
    public bool IsActive => _isActive;

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        _isActive = true;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        _isActive = false;
        return Task.CompletedTask;
    }
}
