using System.Text.Json;

namespace DevOpsQuickScan.Core;

public class QuestionsService(string webrootPath)
{
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true, WriteIndented = true };

    private List<Question> _all = [];

    public async Task<List<Question>> Load()
    {
        var path = Path.Combine(webrootPath, "content", "questions.json");
        if (!File.Exists(path)) 
            throw new Exception("Questions file not found");
        
        await using var stream = File.OpenRead(path);
        _all = await JsonSerializer.DeserializeAsync<List<Question>>(stream, _jsonOptions) ?? [];
        return _all;
    }
}