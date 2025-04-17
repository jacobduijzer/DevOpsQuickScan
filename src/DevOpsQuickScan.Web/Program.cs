using DevOpsQuickScan.Domain;
using DevOpsQuickScan.Web.Components;
using DevOpsQuickScan.Web.Sessions;
using DevOpsQuickScan.Web.Surveys;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddSingleton<ISessionStore, InMemorySessionStore>();
builder.Services.AddScoped<IHubConnectionWrapper, HubConnectionWrapper>();
builder.Services.AddScoped<ISurveyReader, SurveyReader>();
builder.Services.AddScoped<SurveyReader>();
builder.Services.AddScoped<SessionHub>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddSignalR();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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