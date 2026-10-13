#if IOS
using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Orchid.Mobile.Platforms.iOS;

/// <summary>
/// Apple iOS 애플리케이션 수명주기 델리게이트 진입점입니다.
/// </summary>
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    /// <inheritdoc />
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
#endif
