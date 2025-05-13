using DevOpsQuickScan.Web.Components;
using DevOpsQuickScan.Web.Sessions;
using DevOpsQuickScan.Web.Surveys;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBlazorBootstrap();
builder.Services.AddSingleton<ISessionStore, InMemorySessionStore>();
builder.Services.AddScoped<IHubConnectionWrapper, HubConnectionWrapper>();
builder.Services.AddScoped<ISurveyReader, SurveyReader>();
builder.Services.AddScoped<SurveyReader>();
builder.Services.AddScoped<SessionHub>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddSignalR();
// builder.Services.AddSingleton(sp =>
//     new HubConnectionBuilder()
//         .WithUrl(sp.GetRequiredService<NavigationManager>()
//             .ToAbsoluteUri("/hub/voting"))
//         .Build());
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true); // Adjust as needed
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.MapHub<SessionHub>("/hub/voting"); 
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();