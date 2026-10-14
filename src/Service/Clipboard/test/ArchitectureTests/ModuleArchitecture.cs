// keep: Assembly reflection type
using System.Reflection;

namespace Orchid.Service.Clipboard.ArchTests;

/// <summary>
/// Clipboard 서비스의 아키텍처 검증을 위한 어셈블리 및 레이어 네임스페이스 정의 헬퍼입니다.
/// </summary>
internal static class ModuleArchitecture
{
    // ✅ 대상 서비스 어셈블리 및 루트 네임스페이스 지정
    public static readonly Assembly ServiceAssembly = typeof(Domain.Entity.ClipboardBlob).Assembly;
    public const string RootNamespace = "Orchid.Service.Clipboard";

    public static string LayerNamespace(string layer)
    {
        return $"{RootNamespace}.{layer}";
    }
}
