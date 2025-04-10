namespace DevOpsQuickScan.Application;

public interface ISessionStore
{
    string CreateSession(string sessionName);
    string? GetSessionName(string sessionCode);
}