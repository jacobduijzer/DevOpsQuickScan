using DevOpsQuickScan.Api.Domain;

namespace DevOpsQuickScan.Api.Application;

[ExtendObjectType("mutations")]
public class SessionMutations
{
    [GraphQLName("createSession")]
    [GraphQLDescription("Create a new session with a name, a new session code will be generated.")]
    public async Task<SessionCreatedPayload> Create(string sessionName, CancellationToken cancellationToken)
    {
        var sessionCode = CodeGenerator.GenerateCode();
        return new SessionCreatedPayload(sessionCode);
    }
}

public record SessionCreatedPayload(string SessionCode);