namespace DevOpsQuickScan.Domain;

public interface ISessionDataRepository
{
    Task Store(SessionData sessionData);
    Task<SessionData> Retrieve(Guid sessionId);
}