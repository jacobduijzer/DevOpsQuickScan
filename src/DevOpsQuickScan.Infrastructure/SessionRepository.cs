using System.Text;
using System.Text.Json;
using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Infrastructure;

public class SessionRepository(
    string storageAccountName, string storageAccountKey, string containerUrl)
    : BaseRepository(storageAccountName, storageAccountKey, containerUrl), ISessionRepository 
{
    public async Task Save(Session session)
    {   
        var blobClient = ContainerClient.GetBlobClient($"{session.Id}.json");
        var content = JsonSerializer.Serialize(session, JsonSerializerOptions);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        await blobClient.UploadAsync(stream, overwrite: true);
    }

    public async Task<Session> Load(Guid sessionId)
    {
        var blobClient = ContainerClient.GetBlobClient($"{sessionId}.json");

        if (!await blobClient.ExistsAsync())
            throw new FileNotFoundException($"Session file '{sessionId}.json' not found in the sessions blob container.");

        var download = await blobClient.DownloadContentAsync();
        var json = download.Value.Content.ToString();
        
        return JsonSerializer.Deserialize<Session>(json, JsonSerializerOptions) ?? throw new InvalidOperationException("Failed to deserialize question data.");    }
}