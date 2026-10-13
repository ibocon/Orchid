using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Orchid.Service.Clipboard.Infrastructure.Realtime;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

WebApplication app = builder.Build();

app.UseRouting();

app.MapControllers();
app.MapHub<ClipboardHub>("/hubs/clipboard");

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", service = "Orchid.Service.Clipboard" }));

await app.RunAsync().ConfigureAwait(false);
