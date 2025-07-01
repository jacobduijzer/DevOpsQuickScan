using DevOpsQuickScan.Api.Application;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services
    .AddGraphQLServer()
    .AddQueryType(q => q.Name("queries"))
    .AddType<SessionQueries>()
    .AddMutationType(m => m.Name("mutations"))
    .AddType<SessionMutations>();

var app = builder.Build();
app.UseWebSockets();
app.MapGraphQL();
app.Run();