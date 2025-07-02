using DevOpsQuickScan.Application;

namespace DevOpsQuickScan.Api.Application;

[ExtendObjectType("mutations")]
public class SessionMutations
{
    [GraphQLName("createSession")]
    [GraphQLDescription("Create a new session with a name, a new session code will be generated.")]
    public async Task<SessionCreatedPayload> Create(string sessionName, CancellationToken cancellationToken, [Service] CreateSessionUseCase useCase)
    {
        var command = new CreateSessionUseCase.CreateSessionCommand(sessionName);
        var sessionCode = await useCase.Handle(command);
        return new SessionCreatedPayload(sessionCode);
    }
}

public record SessionCreatedPayload(string SessionCode);