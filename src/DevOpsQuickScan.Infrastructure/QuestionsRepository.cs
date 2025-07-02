using System.Text.Json;
using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Infrastructure;

public class QuestionsRepository : IQuestionsRepository
{
    private JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        IncludeFields = true
    };
    
    public async Task<List<Question>> All()
    {
        if(!File.Exists("App_Data/questions.json"))
            throw new FileNotFoundException("Questions file not found.");
        
        var json = await File.ReadAllTextAsync("App_Data/questions.json");
        
        return JsonSerializer.Deserialize<List<Question>>(json)!
            ?? throw new Exception("Cannot deserialize questions.");;
    }
}