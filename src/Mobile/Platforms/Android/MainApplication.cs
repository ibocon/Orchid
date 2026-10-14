#if ANDROID
using Android.App;
using Android.Runtime;

namespace Orchid.Mobile.Platforms.Android;

/// <summary>
/// Android 플랫폼의 메인 애플리케이션 진입점입니다.
/// </summary>
[Application]
public class MainApplication : MauiApplication
{
    /// <summary>
    /// <see cref="MainApplication"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public MainApplication(nint handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    /// <inheritdoc />
    protected override MauiApp CreateMauiApp()
    {
        return MauiProgram.CreateMauiApp();
    }
}
#endif
