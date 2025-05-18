using System.Text.Json;
using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Infrastructure;

public class QuestionRepository(
    string storageAccountName, string storageAccountKey, string containerUrl)
    : BaseRepository(storageAccountName, storageAccountKey, containerUrl), IQuestionRepository
{
    public async Task<QuestionData> Get()
    {
        var blobClient = ContainerClient.GetBlobClient("questions.json");

        if (!await blobClient.ExistsAsync())
            throw new FileNotFoundException("Questions file 'questions.json' not found in the questions blob container.");

        var download = await blobClient.DownloadContentAsync();
        var json = download.Value.Content.ToString();
        
        return JsonSerializer.Deserialize<QuestionData>(json, JsonSerializerOptions) ?? throw new InvalidOperationException("Failed to deserialize question data.");
    }
}