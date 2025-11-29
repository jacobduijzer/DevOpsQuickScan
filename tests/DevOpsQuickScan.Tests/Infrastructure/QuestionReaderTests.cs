using DevOpsQuickScan.Infrastructure.Sessions;

namespace DevOpsQuickScan.Tests.Infrastructure;

public class QuestionReaderTests
{
    [Fact]
    public async Task ReturnsQuestions()
    {
        // ARRANGE
        QuestionReader reader = new QuestionReader("Core");
        
        // ACT
        var questions = await reader.Read();
        
        // ASSERT
        Assert.NotEmpty(questions);
        Assert.Equal(2, questions.Count);
    }
}