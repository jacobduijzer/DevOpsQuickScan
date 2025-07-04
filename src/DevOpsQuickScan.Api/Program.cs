using DevOpsQuickScan.Api.Application;
using DevOpsQuickScan.Application;
using DevOpsQuickScan.Domain;
using DevOpsQuickScan.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services
    .AddScoped<CreateSessionCommandHandler>()
    .AddScoped<NextQuestionQueryHandler>()
    .AddScoped<PreviousQuestionQueryHandler>()
    .AddScoped<AskQuestionCommandHandler>()
    .AddScoped<SessionService>()
    .AddScoped<IQuestionSender, QuestionSender>()
    .AddScoped<IAnswersSender, AnswerSender>()
    .AddScoped<ISessionDataRepository, SessionDataRepository>()
    .AddScoped<IQuestionsRepository, QuestionsRepository>();

builder.Services
    .AddGraphQLServer()
    .AddType<QuestionType>()    
    .AddQueryType(q => q.Name("queries"))
    .AddType<SessionQueries>()
    .AddMutationType(m => m.Name("mutations"))
    .AddType<SessionMutations>();

var app = builder.Build();
app.UseWebSockets();
app.MapGraphQL();
app.Run();