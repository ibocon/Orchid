// keep: FluentAssertions extension methods
using FluentAssertions;
// keep: NetArchTest fluent rules
using NetArchTest.Rules;
// keep: xUnit Fact attribute
using Xunit;

namespace Orchid.ArchTests;

/// <summary>
/// Orchid.Contract 라이브러리의 솔루션 내외부 의존성 제로(순수 BCL 계약) 보장을 검증하는 아키텍처 테스트입니다.
/// </summary>
public sealed class ContractPurityTest
{
    /// <summary>
    /// Contract 라이브러리가 솔루션 내 다른 프로젝트에 의존하지 않는지 검증합니다.
    /// </summary>
    [Fact]
    public void Contract_ShouldNotDependOn_InternalProjects()
    {
        // Given
        var contractAssembly = typeof(Orchid.Contract.Payload.EncryptedPayload).Assembly;

        // When
        // ✅ Orchid.Contract는 솔루션 내 타 프로젝트에 의존해서는 안 됨을 강제
        TestResult result = Types.InAssembly(contractAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Orchid.Core",
                "Orchid.Desktop",
                "Orchid.Mobile",
                "Orchid.Web",
                "Orchid.Service")
            .GetResult();

        // Then
        result.IsSuccessful.Should().BeTrue("Contract 라이브러리는 100% 독립적인 순수 계약이어야 합니다.");
    }
}
