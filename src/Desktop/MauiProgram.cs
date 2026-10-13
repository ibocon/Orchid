using Microsoft.Extensions.Logging;
using Orchid.Core.Abstraction;
using Orchid.Core.Cryptography;
using Orchid.Core.Storage;
using Orchid.Core.Sync;
using Orchid.Desktop.Service;

namespace Orchid.Desktop;

/// <summary>
/// .NET MAUI 데스크톱 애플리케이션 진입점 및 의존성 주입을 구성합니다.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// MAUI 앱 호스트를 빌드하고 초기화합니다.
    /// </summary>
    /// <returns>구성된 <see cref="MauiApp"/> 인스턴스</returns>
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddSingleton<ISystemClipboard, DesktopClipboardAdapter>();
        builder.Services.AddSingleton<IAesGcmCryptoService, AesGcmCryptoService>();

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "orchid.db");
        builder.Services.AddSingleton<IClipboardRepository>(_ => new SqliteClipboardRepository(dbPath));
        builder.Services.AddSingleton<ISyncEngine, SyncEngine>();

        return builder.Build();
    }
}
