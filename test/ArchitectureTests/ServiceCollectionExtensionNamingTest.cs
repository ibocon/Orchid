// keep: Assembly reflection type
using System.Reflection;
// keep: FluentAssertions extension methods
using FluentAssertions;
// keep: IServiceCollection extension type
using Microsoft.Extensions.DependencyInjection;
// keep: xUnit Fact attribute
using Xunit;

namespace Orchid.ArchTests;

/// <summary>
/// 솔루션 전역 IServiceCollection 확장 메서드의 명명 규약(Add 접두사 및 중복 접미사 배제)을 검증하는 아키텍처 테스트입니다.
/// </summary>
public sealed class ServiceCollectionExtensionNamingTest
{
    private static readonly string[] RedundantSuffixes = ["Services", "Vendors", "Sinks"];

    private static IEnumerable<MethodInfo> GetServiceCollectionExtensionMethods()
    {
        Assembly[] assemblies =
        [
            typeof(Orchid.Core.Domain.Entity.ClipboardItem).Assembly,
            typeof(Orchid.Service.Clipboard.Domain.Entity.ClipboardBlob).Assembly,
            typeof(Orchid.ApiGateway.Middleware.CorrelationIdMiddleware).Assembly
        ];

        return assemblies
            .SelectMany(a => a.GetExportedTypes())
            .Where(t => t.IsAbstract && t.IsSealed) // 정적 클래스
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
            .Where(m => m.GetParameters().Length > 0 &&
                        m.GetParameters()[0].ParameterType == typeof(IServiceCollection));
    }

    /// <summary>
    /// 모든 IServiceCollection 확장 메서드는 Add 접두사로 시작해야 함을 검증합니다.
    /// </summary>
    [Fact]
    public void ExtensionMethodsShouldStartWithAdd()
    {
        // Given & When
        // ✅ 모든 IServiceCollection 확장 메서드는 Add로 시작해야 함을 검증
        string[] violations = GetServiceCollectionExtensionMethods()
            .Where(method => !method.Name.StartsWith("Add", StringComparison.Ordinal))
            .Select(method => $"{method.DeclaringType?.FullName}.{method.Name}")
            .ToArray();

        // Then
        violations.Should().BeEmpty("ServiceCollectionExtensions 메서드는 반드시 Add 접두사로 시작해야 합니다.");
    }

    /// <summary>
    /// 확장 메서드 이름에 Services, Vendors, Sinks 중복 접미사가 포함되지 않음을 검증합니다.
    /// </summary>
    [Fact]
    public void ExtensionMethodsShouldNotUseRedundantSuffix()
    {
        // Given & When
        // ✅ 확장 메서드 이름에 Services, Vendors, Sinks 중복 접미사 금지 검증
        string[] violations = GetServiceCollectionExtensionMethods()
            .Where(method => RedundantSuffixes.Any(suffix => method.Name.EndsWith(suffix, StringComparison.Ordinal)))
            .Select(method => $"{method.DeclaringType?.FullName}.{method.Name}")
            .ToArray();

        // Then
        violations.Should().BeEmpty("ServiceCollectionExtensions 메서드는 불필요한 복수형 접미사를 포함해서는 안 됩니다.");
    }
}
