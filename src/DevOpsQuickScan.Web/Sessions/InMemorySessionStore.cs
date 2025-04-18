namespace DevOpsQuickScan.Web.Sessions;

public class InMemorySessionStore : ISessionStore
{
    private readonly Dictionary<string, string> _sessions = new();

    public string CreateSession(string sessionName)
    {
        var code = Guid.NewGuid().ToString("N")[..10]; 
        _sessions[code] = sessionName;
        return code;
    }

    public string? GetSessionName(string sessionCode)
    {
        return _sessions.TryGetValue(sessionCode, out var name) ? name : null;
    } 
}