using DevOpsQuickScan.Core;

namespace DevOpsQuickScan.Tests.Core;

public class Export_Service_Should
{
    [Fact]
    public async Task Export_Session_Data_As_Csv()
    {
        // ARRANGE
        ExportService exportService = new();
        QuestionsService questionsService = new("Core");
        var questions = await questionsService.Load();
        List<AnswerSubmission> answers =
        [
            new AnswerSubmission(Guid.NewGuid().ToString(), 1, 1),
            new AnswerSubmission(Guid.NewGuid().ToString(), 1, 2),
            new AnswerSubmission(Guid.NewGuid().ToString(), 1, 2),
            new AnswerSubmission(Guid.NewGuid().ToString(), 2, 3),
            new AnswerSubmission(Guid.NewGuid().ToString(), 2, 3),
        ];
        
        
        // ACT
        var export = exportService.ExportToCsv(questions, answers);

        // ASSERT
        Assert.NotEmpty(export);
        Assert.Equal("QuestionText,Answer 1,Votes,Answer 2, Votes, Answer 3, Votes, Answer 4, Votes, Answer 5, Votes, Total Votes\n\"How does your team ensure that code remains maintainable over time?\",\"We don’t have a defined approach\",1,\"We use basic linting and formatting tools\",2,\"We follow code review and documentation guidelines\",0,\"We apply automated maintainability checks and refactoring practices\",0,\"We have a culture of continuous refactoring and evolutionary architecture\",0,3\n\"How does your team ensure the quality and usefulness of documentation?\",\"We rarely document or our documentation is outdated\",0,\"We create basic documentation for major features when needed\",0,\"We maintain up-to-date documentation for key systems and processes\",2,\"We have standards for documentation quality and regularly review it\",0,\"We treat documentation as a core deliverable, with clear ownership, continuous improvement, and easy discoverability\",0,2\n", export);
    }
}