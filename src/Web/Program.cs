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
builder.Services.AddScoped<ISystemClipboard, WebClipboardAdapter>();
builder.Services.AddSingleton<IAesGcmCryptoService, AesGcmCryptoService>();

await builder.Build().RunAsync();
