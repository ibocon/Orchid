namespace Orchid.Contract.Model;

/// <summary>
/// 지원되는 클라이언트 OS 디바이스 플랫폼을 정의합니다.
/// </summary>
public enum DevicePlatform
{
    /// <summary>
    /// Microsoft Windows
    /// </summary>
    Windows = 1,

    /// <summary>
    /// Apple macOS (MacCatalyst)
    /// </summary>
    MacCatalyst = 2,

    /// <summary>
    /// Google Android
    /// </summary>
    Android = 3,

    /// <summary>
    /// Apple iOS
    /// </summary>
    Ios = 4,

    /// <summary>
    /// 웹 브라우저 (Blazor WebAssembly)
    /// </summary>
    Web = 5
}
