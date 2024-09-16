using StockView.Components;
using StockView.Models;
using StockView.Services;
using StockView.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// register FinnHub
builder.Services.AddOptions();
builder.Services.Configure<FinnHub>(builder.Configuration.GetSection("FinnHub"));

// register FinnHubService
builder.Services.AddScoped<IFinnHubService, FinnHubService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();