using System.Text.Json;
using DevOpsQuickScan.Domain.Questions;
using DevOpsQuickScan.Domain.Sessions;

namespace DevOpsQuickScan.Infrastructure.Sessions;

public class QuestionReader(string webrootPath) : IQuestionReader
{
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true, WriteIndented = true };
    private List<Question> _all = [];

    public async Task<List<Question>> Read()
    {
        var path = Path.Combine(webrootPath, "content", "questions.json");
        if (!File.Exists(path)) 
            throw new Exception("Questions file not found");
        
        await using var stream = File.OpenRead(path);
        _all = await JsonSerializer.DeserializeAsync<List<Question>>(stream, _jsonOptions) ?? [];
        return _all;
    }
}