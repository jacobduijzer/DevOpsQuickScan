using System.Text.Json;

namespace DevOpsQuickScan.Web.Surveys;

public class SurveyReader : ISurveyReader
{
    public async Task<Survey> Read(string fileName)
    {
        var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        var jsonFile = Path.Combine(path, fileName);
        if (!File.Exists(fileName))
            throw new FileNotFoundException("File not found", jsonFile);

        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonSurveyData = await File.ReadAllTextAsync(jsonFile);
        return JsonSerializer.Deserialize<Survey>(jsonSurveyData, options)!;
    }
}