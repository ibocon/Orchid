#if ANDROID
using Android.AccessibilityServices;
using Android.Views.Accessibility;

namespace Orchid.Mobile.Platforms.Android;

/// <summary>
/// Android 10+ 백그라운드 클립보드 접근 제한을 극복하기 위한 접근성 서비스 구현체입니다.
/// </summary>
[global::Android.App.Service(
    Label = "Orchid Clipboard Sync Service",
    Permission = "android.permission.BIND_ACCESSIBILITY_SERVICE",
    Exported = true)]
public class ClipboardAccessibilityService : AccessibilityService
{
    /// <inheritdoc />
    public override void OnAccessibilityEvent(AccessibilityEvent? e)
    {
        // 텍스트 포커스 및 클립보드 변경 이벤트 감지
    }

    /// <inheritdoc />
    public override void OnInterrupt()
    {
    }
}
#endif
