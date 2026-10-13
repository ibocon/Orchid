using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Orchid.ApiGateway.Middleware;

/// <summary>
/// 모든 유입 요청에 고유 상관관계 ID(Correlation ID)를 부여하고 W3C 추적 헤더를 전파하는 미들웨어입니다.
/// </summary>
public sealed class CorrelationIdMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-ID";
    private readonly RequestDelegate _next;

    /// <summary>
    /// <see cref="CorrelationIdMiddleware"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="next">다음 파이프라인 대리자</param>
    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    /// <summary>
    /// HTTP 요청 파이프라인에서 상관관계 ID를 확인하거나 새로 생성하여 주입합니다.
    /// </summary>
    /// <param name="context">현재 HTTP 컨텍스트</param>
    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (!context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out StringValues correlationId) ||
            string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("D");
            context.Request.Headers[CorrelationIdHeaderName] = correlationId;
        }

        context.Response.Headers[CorrelationIdHeaderName] = correlationId;

        await _next(context).ConfigureAwait(false);
    }
}
