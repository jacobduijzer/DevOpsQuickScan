using DevOpsQuickScan.Domain;
using DevOpsQuickScan.Infrastructure;
using DevOpsQuickScan.Web.Components;
using DevOpsQuickScan.Web.Sessions;
using DevOpsQuickScan.Web.Surveys;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBlazorBootstrap();
builder.Services.AddSingleton<ISessionStore, InMemorySessionStore>();
builder.Services.AddScoped<IHubConnectionWrapper, HubConnectionWrapper>();
builder.Services.AddScoped<ISurveyReader, SurveyReader>();
builder.Services.AddScoped<SurveyReader>();
builder.Services.AddScoped<SessionHub>();
builder.Services.AddScoped<CurrentSessionService>();
builder.Services.AddScoped<IQuestionRepository>(sp =>
{
    var storageAccountName = builder.Configuration["BLOB_STORAGE_ACCOUNT_NAME"] ?? throw new InvalidOperationException("BLOB_STORAGE_ACCOUNT_NAME is not set.");
    var storageAccountKey = builder.Configuration["BLOB_STORAGE_SHARED_ACCESS_KEY"] ?? throw new InvalidOperationException("BLOB_STORAGE_SHARED_ACCESS_KEY is not set.");
    var questionsContainerName = builder.Configuration["BLOG_STORAGE_QUESTIONS_CONTAINER_URL"] ?? throw new InvalidOperationException("BLOG_STORAGE_QUESTIONS_CONTAINER_URL is not set.");
    return new QuestionRepository(storageAccountName, storageAccountKey, questionsContainerName);
});
builder.Services.AddScoped<ISessionRepository>(sp =>
{
    var storageAccountName = builder.Configuration["BLOB_STORAGE_ACCOUNT_NAME"] ?? throw new InvalidOperationException("BLOB_STORAGE_ACCOUNT_NAME is not set.");
    var storageAccountKey = builder.Configuration["BLOB_STORAGE_SHARED_ACCESS_KEY"] ?? throw new InvalidOperationException("BLOB_STORAGE_SHARED_ACCESS_KEY is not set.");
    var sessionsContainerName = builder.Configuration["BLOG_STORAGE_SESSION_CONTAINER_URL"] ?? throw new InvalidOperationException("BLOG_STORAGE_QUESTIONS_CONTAINER_URL is not set.");
    return new SessionRepository(storageAccountName, storageAccountKey, sessionsContainerName);
});
builder.Services.AddSignalR();
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