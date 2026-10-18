using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Orchid.UnitTests.Mobile;

/// <summary>
/// Mobile 호스트 페이지가 Android에서 Blazor 자동 시작을 막는 스크립트 태그를 유지하는지 검증하는 단위 테스트입니다.
/// </summary>
public sealed class MobileHostPageTests
{
    [Fact]
    public void IndexHtml_When_Blazor_Script_Loaded_Should_Defer_And_Disable_Autostart()
    {
        // Given
        string html = File.ReadAllText(FindRepositoryFile("src/Mobile/wwwroot/index.html"));

        // When
        Match script = Regex.Match(html, "<script[^>]*src=\"_framework/blazor\\.webview\\.js\"[^>]*>");

        // Then
        script.Success.Should().BeTrue();
        script.Value.Should().Contain("defer");
        script.Value.Should().Contain("autostart=\"false\"");
    }

    private static string FindRepositoryFile(string relativePath)
    {
        DirectoryInfo? directory = new(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "Orchid.slnx")))
        {
            directory = directory.Parent;
        }

        return Path.Combine(directory?.FullName ?? throw new InvalidOperationException("Orchid.slnx not found."), relativePath);
    }
}
