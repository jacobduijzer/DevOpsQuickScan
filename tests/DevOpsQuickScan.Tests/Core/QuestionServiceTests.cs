using DevOpsQuickScan.Core;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace DevOpsQuickScan.Tests.Core;

public class QuestionServiceTests
{
    [Fact]
    public async Task CanLoadQuestionsFromDisk()
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
    public async Task ThrowsWhenFileCanNotBeFound()
    {
        // ARRANGE
        QuestionsService questionsService = new("./");

        // ACT & ASSERT
        await Assert.ThrowsAsync<Exception>(async () => await questionsService.Load());
    }
}
