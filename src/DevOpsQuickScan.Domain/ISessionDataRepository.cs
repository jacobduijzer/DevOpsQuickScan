namespace DevOpsQuickScan.Domain;

public interface ISessionDataRepository
{
    Task Store(SessionData sessionData);
    Task<SessionData> Retrieve(string sessionCode);
}