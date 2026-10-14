using MudBlazor.Services;
using Garden.Razor;
using Orchid.Core.Cryptography;
using Orchid.Core.Storage;
using Orchid.Core.Sync;

namespace Orchid.Mobile;

/// <summary>
/// .NET MAUI 모바일 애플리케이션 진입점 및 의존성 주입을 구성합니다.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// 모바일 MAUI 앱 호스트를 빌드하고 초기화합니다.
    /// </summary>
    /// <returns>구성된 <see cref="MauiApp"/> 인스턴스</returns>
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();
        builder.Services.AddGardenRazor();
        builder.Services.AddDesignSystem();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddSingleton<IAesGcmCryptoService, AesGcmCryptoService>();

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "orchid_mobile.db");
        builder.Services.AddSingleton<IClipboardRepository>(_ => new SqliteClipboardRepository(dbPath));
        builder.Services.AddSingleton<ISyncEngine, SyncEngine>();

        return builder.Build();
    }
}
