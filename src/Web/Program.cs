using MudBlazor.Services;
using Garden.Razor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Orchid.Core.Abstraction;
using Orchid.Core.Cryptography;
using Orchid.Web;
using Orchid.Web.Service;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddGardenRazor();
builder.Services.AddDesignSystem();
builder.Services.AddScoped<ISystemClipboard, WebClipboardAdapter>();
builder.Services.AddSingleton<IAesGcmCryptoService, AesGcmCryptoService>();

await builder.Build().RunAsync().ConfigureAwait(false);
