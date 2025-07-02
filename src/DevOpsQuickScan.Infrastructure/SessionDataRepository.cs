using System.Text.Json;
using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Infrastructure;

public class SessionDataRepository : ISessionDataRepository
{
    public async Task Store(SessionData sessionData)
    {
        var json = JsonSerializer.Serialize(sessionData, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync($"App_Data/Sessions/{sessionData.SessionCode}.json", json);
    }

    public async Task<SessionData> Retrieve(string sessionCode)
    {
        var fileName = $"App_Data/Sessions/{sessionCode}.json";
        if (!File.Exists(fileName))
            throw new FileNotFoundException("Session data file not found.", fileName);

        var json = await File.ReadAllTextAsync(fileName);
        return JsonSerializer.Deserialize<SessionData>(json) ?? throw new Exception("Cannot deserialize session data.");
    }
}