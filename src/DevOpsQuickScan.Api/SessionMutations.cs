using DevOpsQuickScan.Application;
using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Api.Application;

[ExtendObjectType("mutations")]
public class SessionMutations
{
    [GraphQLName("createSession")]
    [GraphQLDescription("Create a new session with a name, a new session code will be generated.")]
    public async Task<SessionCreatedPayload> Create(string sessionName, [Service] CreateSessionCommandHandler handler)
    {
        var command = new CreateSessionCommandHandler.CreateSessionCommand(sessionName);
        var sessionCode = await handler.Handle(command);
        return new SessionCreatedPayload(sessionCode);
    }

    [GraphQLName("askQuestion")]
    public async Task<AskQuestionPayload> Ask(string sessionCode, [Service] AskQuestionCommandHandler handler)
    {
        var command = new AskQuestionCommandHandler.AskQuestionCommand(sessionCode);
        var question = await handler.Handle(command);
        return new AskQuestionPayload(sessionCode, question);
    }
}

public record SessionCreatedPayload(string SessionCode);

public record AskQuestionPayload(string SessionCode, Question Question);