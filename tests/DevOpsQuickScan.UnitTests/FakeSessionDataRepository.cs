using System.Text.Json;
using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.UnitTests;

public class FakeSessionDataRepository : ISessionDataRepository
{
    private string? _sessionDataJson;
    
    public async Task Store(SessionData sessionData)
    {
        _sessionDataJson = JsonSerializer.Serialize(sessionData);
        await Task.CompletedTask; 
    }

    public async Task<SessionData> Retrieve(string sessionCode)
    {
        var sessionData = JsonSerializer.Deserialize<SessionData>(_sessionDataJson!);
        return await Task.FromResult(sessionData!);
    }
}