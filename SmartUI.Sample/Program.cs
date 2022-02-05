using AntiRap.Core.Abstractions;
using AntiRap.Core.Utilities;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartUI.Blazor.Data.Shared;
using SmartUI.Sample;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<IPropertyHandler, PropertyHandler>();
builder.Services.AddScoped(typeof(IHttpFeatureService<>), typeof(HttpFeatureService<>));
await builder.Build().RunAsync();
