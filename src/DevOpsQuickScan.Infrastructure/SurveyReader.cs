using System.Text.Json;
using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Infrastructure;

public class SurveyReader
{
    public async Task<Survey> Read(string fileName)
    {
        var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        var jsonFile = await File.ReadAllTextAsync(Path.Combine(path, fileName));
        if (!File.Exists(fileName))
            throw new FileNotFoundException("File not found", jsonFile);

        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonSurveyData = await File.ReadAllTextAsync(fileName);
        return JsonSerializer.Deserialize<Survey>(jsonSurveyData, options)!;
    }
}