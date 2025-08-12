using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace DevOpsQuickScan.Core;

public class QuestionsService(IWebHostEnvironment env)
{
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true, WriteIndented = true };

    private List<Question> _all = [];

    public async Task<List<Question>> Load()
    {
        var path = Path.Combine(env.WebRootPath, "content", "questions.json");
        if (!File.Exists(path)) 
            throw new Exception("Questions file not found");
        
        await using var stream = File.OpenRead(path);
        _all = await JsonSerializer.DeserializeAsync<List<Question>>(stream, _jsonOptions) ?? [];
        return _all;
    }
    
    // public void RevealQuestion(int questionId)
    // {
    //     var question = _all.FirstOrDefault(q => q.Id == questionId);
    //     if (question is not null)
    //         question.IsRevealed = true;
    // }
    //
    // public void ResetQuestion(int questionId)
    // {
    //     var question = _all.FirstOrDefault(q => q.Id == questionId);
    //     if (question is not null)
    //         question.IsRevealed = false;
    // }
}