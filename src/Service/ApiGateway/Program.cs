using Orchid.ApiGateway.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

WebApplication app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", service = "Orchid.ApiGateway" }));

app.MapReverseProxy();

await app.RunAsync().ConfigureAwait(false);
