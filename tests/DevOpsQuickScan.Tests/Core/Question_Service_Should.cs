using DevOpsQuickScan.Core;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace DevOpsQuickScan.Tests.Core;

public class QuestionServiceShould
{
    [Fact]
    public async Task Load_Questions_From_Disk()
    {
        // ARRANGE
        QuestionsService questionsService = new("Core");

        // ACT
        var questions = await questionsService.Load();

        // ASSERT
        Assert.NotNull(questions);
        Assert.Equal(2, questions.Count);
    }
    
    [Fact]
    public async Task Throw_Exception_When_File_Can_Not_Be_Found()
    {
        // ARRANGE
        QuestionsService questionsService = new("./");

        // ACT & ASSERT
        await Assert.ThrowsAsync<Exception>(async () => await questionsService.Load());
    }
}
