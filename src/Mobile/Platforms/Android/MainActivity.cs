#if ANDROID
using Android.App;
using Android.Content.PM;
using Microsoft.Maui;

namespace Orchid.Mobile.Platforms.Android;

/// <summary>
/// Orchid Android 메인 액티비티 진입점입니다.
/// </summary>
[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
public class MainActivity : MauiAppCompatActivity
{
}
#endif
