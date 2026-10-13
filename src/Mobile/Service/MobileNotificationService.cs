using Orchid.Core.Abstraction;

namespace Orchid.Mobile.Service;

/// <summary>
/// 모바일 네이티브 푸시 및 로컬 배너 알림 서비스 구현체입니다.
/// </summary>
public sealed class MobileNotificationService : INativeNotification
{
    /// <inheritdoc />
    public Task ShowNotificationAsync(string title, string message)
    {
        // OS 네이티브 알림 채널 발행
        return Task.CompletedTask;
    }
}
