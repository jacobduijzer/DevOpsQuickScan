namespace DevOpsQuickScan.Web.Sessions;

public interface ISessionStore
{
    string CreateSession(string sessionName);
    string? GetSessionName(string sessionCode);
}