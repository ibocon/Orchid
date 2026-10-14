using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Orchid.ApiGateway.Middleware;
using Xunit;

namespace Orchid.UnitTests.Gateway;

/// <summary>
/// <see cref="CorrelationIdMiddleware"/>의 고유 식별자 발급 및 기존 식별자 전파를 검증하는 단위 테스트입니다.
/// </summary>
public sealed class CorrelationIdMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenHeaderMissing_ShouldGenerateAndAttachCorrelationId()
    {
        // Given
        var context = new DefaultHttpContext();
        var middleware = new CorrelationIdMiddleware(next: (_) => Task.CompletedTask);

        // When
        await middleware.InvokeAsync(context);

        // Then
        context.Request.Headers.ContainsKey("X-Correlation-ID").Should().BeTrue();
        context.Response.Headers.ContainsKey("X-Correlation-ID").Should().BeTrue();
        Guid.TryParse(context.Response.Headers["X-Correlation-ID"].ToString(), out _).Should().BeTrue();
    }

    [Fact]
    public async Task InvokeAsync_WhenHeaderPresent_ShouldPreserveCorrelationId()
    {
        // Given
        string existingId = "existing-custom-trace-id";
        var context = new DefaultHttpContext();
        context.Request.Headers["X-Correlation-ID"] = existingId;

        var middleware = new CorrelationIdMiddleware(next: (_) => Task.CompletedTask);

        // When
        await middleware.InvokeAsync(context);

        // Then
        context.Response.Headers["X-Correlation-ID"].ToString().Should().Be(existingId);
    }
}
