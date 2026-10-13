using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Orchid.ApiGateway.Middleware;
using Xunit;

namespace Orchid.UnitTests.Gateway;

public sealed class CorrelationIdMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenHeaderMissing_ShouldGenerateAndAttachCorrelationId()
    {
        // Given
        var context = new DefaultHttpContext();
        var nextCalled = false;
        var middleware = new CorrelationIdMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });

        // When
        await middleware.InvokeAsync(context);

        // Then
        nextCalled.Should().BeTrue();
        context.Response.Headers.Should().ContainKey("X-Correlation-ID");
        var correlationId = context.Response.Headers["X-Correlation-ID"].ToString();
        correlationId.Should().NotBeNullOrWhiteSpace();
        Guid.TryParse(correlationId, out _).Should().BeTrue();
    }

    [Fact]
    public async Task InvokeAsync_WhenHeaderPresent_ShouldPreserveCorrelationId()
    {
        // Given
        const string existingCorrelationId = "test-correlation-123";
        var context = new DefaultHttpContext();
        context.Request.Headers["X-Correlation-ID"] = existingCorrelationId;

        var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask);

        // When
        await middleware.InvokeAsync(context);

        // Then
        context.Response.Headers["X-Correlation-ID"].ToString().Should().Be(existingCorrelationId);
    }
}
