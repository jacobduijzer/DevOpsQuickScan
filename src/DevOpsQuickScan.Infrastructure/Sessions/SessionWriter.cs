using System.Text.Json;
using DevOpsQuickScan.Domain.Sessions;

namespace DevOpsQuickScan.Infrastructure.Sessions;

public class SessionWriter(string webrootPath) : ISessionWriter
{
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true, WriteIndented = true };
    public async Task Write(Session session)
    {
       var sessionJson = JsonSerializer.Serialize(session, _jsonOptions);
       await File.WriteAllTextAsync(Path.Combine(webrootPath, "content", "sessions", $"{session.Id}.json"), sessionJson);
    }
    
}