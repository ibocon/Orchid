// keep: FluentAssertions extension methods
using FluentAssertions;
// keep: NetArchTest fluent rules
using NetArchTest.Rules;
// keep: xUnit Fact attribute
using Xunit;

namespace Orchid.Service.Clipboard.ArchTests;

/// <summary>
/// Clipboard 서비스의 Clean Architecture 계층 의존성 및 CQRS 핸들러 불변식을 검증하는 아키텍처 테스트입니다.
/// </summary>
public sealed class ModuleArchitectureRulesTest
{
    /// <summary>
    /// 도메인 계층이 외부 계층(Application, Infrastructure, Api)에 의존하지 않는지 검증합니다.
    /// </summary>
    [Fact]
    public void DomainLayer_ShouldNotDependOn_OuterLayers()
    {
        // Given & When
        // ✅ Domain 계층이 Application, Infrastructure, Api 계층에 의존하지 않음을 강제
        TestResult result = Types.InAssembly(ModuleArchitecture.ServiceAssembly)
            .That()
            .ResideInNamespace(ModuleArchitecture.LayerNamespace("Domain"))
            .ShouldNot()
            .HaveDependencyOnAny(
                ModuleArchitecture.LayerNamespace("Application"),
                ModuleArchitecture.LayerNamespace("Infrastructure"),
                ModuleArchitecture.LayerNamespace("Api"))
            .GetResult();

        // Then
        result.IsSuccessful.Should().BeTrue("도메인 계층은 외부 계층에 의존해서는 안 됩니다.");
    }

    /// <summary>
    /// 애플리케이션 계층이 외부 계층(Infrastructure, Api)에 의존하지 않는지 검증합니다.
    /// </summary>
    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_InfrastructureOrApi()
    {
        // Given & When
        // ✅ Application 계층이 Infrastructure 및 Api 계층에 직접 의존하지 않음을 강제
        TestResult result = Types.InAssembly(ModuleArchitecture.ServiceAssembly)
            .That()
            .ResideInNamespace(ModuleArchitecture.LayerNamespace("Application"))
            .ShouldNot()
            .HaveDependencyOnAny(
                ModuleArchitecture.LayerNamespace("Infrastructure"),
                ModuleArchitecture.LayerNamespace("Api"))
            .GetResult();

        // Then
        result.IsSuccessful.Should().BeTrue("애플리케이션 계층은 인프라 및 API 계층에 의존해서는 안 됩니다.");
    }

    /// <summary>
    /// MediatR 핸들러 간 직접 호출이 없으며 VSA 독립성이 보장되는지 검증합니다.
    /// </summary>
    [Fact]
    public void Handlers_ShouldNotDependOn_OtherHandlers()
    {
        // Given & When
        // ✅ MediatR 핸들러 간 직접 호출을 금지하여 VSA 독립성 보장
        Type[] handlerTypes = Types.InAssembly(ModuleArchitecture.ServiceAssembly)
            .That()
            .HaveNameEndingWith("Handler")
            .GetTypes()
            .ToArray();

        foreach (Type handlerType in handlerTypes)
        {
            string[] otherHandlerNames = handlerTypes
                .Where(t => t != handlerType)
                .Select(static t => t.FullName)
                .Where(static name => name is not null)
                .Cast<string>()
                .ToArray();

            if (otherHandlerNames.Length == 0)
            {
                continue;
            }

            TestResult result = Types.InAssembly(ModuleArchitecture.ServiceAssembly)
                .That()
                .HaveName(handlerType.Name)
                .ShouldNot()
                .HaveDependencyOnAny(otherHandlerNames)
                .GetResult();

            result.IsSuccessful.Should().BeTrue("핸들러 간 직접 의존은 금지되며 별도 커맨드로 분리해야 합니다.");
        }
    }

    /// <summary>
    /// 도메인 및 애플리케이션 계층에 외부 DB SDK(Microsoft.EntityFrameworkCore, Npgsql)가 침투하지 않는지 검증합니다.
    /// </summary>
    [Fact]
    public void DomainAndApplicationLayers_ShouldNotDependOn_ExternalDatabaseSdks()
    {
        // Given & When
        // ✅ 도메인 및 애플리케이션 계층에 외부 DB SDK 유입 원천 차단
        TestResult result = Types.InAssembly(ModuleArchitecture.ServiceAssembly)
            .That()
            .ResideInNamespace(ModuleArchitecture.LayerNamespace("Domain"))
            .Or()
            .ResideInNamespace(ModuleArchitecture.LayerNamespace("Application"))
            .ShouldNot()
            .HaveDependencyOnAny(
                "Microsoft.EntityFrameworkCore",
                "Npgsql")
            .GetResult();

        // Then
        result.IsSuccessful.Should().BeTrue("도메인 및 애플리케이션 계층은 외부 DB SDK에 직접 의존해서는 안 됩니다.");
    }
}
