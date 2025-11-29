using DevOpsQuickScan.BlazorApp;
using DevOpsQuickScan.BlazorApp.Components;
using DevOpsQuickScan.Core;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services
    .AddApplicationInsightsTelemetry()
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddSingleton<QuestionsService>(sp =>
    {
        var webrootPath = sp.GetRequiredService<IWebHostEnvironment>().WebRootPath;
        return new QuestionsService(webrootPath);
    })
    .AddSingleton<SessionService>()
    .AddSingleton<ExportService>()
    .AddScoped<IUserIdService, UserIdService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();