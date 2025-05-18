using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.UnitTests.Stubs;

public class SessionRepositoryStub : ISessionRepository
{
    private Session? _session;
    
    public Task Save(Session session)
    {
        _session = session;
        return Task.CompletedTask;
    }

    public Task<Session> Load(Guid sessionId)
    {
        return Task.FromResult(_session!);
    }
}