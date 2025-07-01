using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.UnitTests.Domain;

public class SessionServiceTests_Questions
{
    [Fact]
    public void CanNotGoToPreviousQuestionWhenStarting()
    {

        // ARRANGE
        SessionService sessionService = new();
        sessionService.Start("Test Session",
            [new Question(1, "What is your favorite color?"), new Question(2, "What is your favorite food?")]);

        // ACT
        var question = sessionService.PreviousQuestion();

        // ASSERT
        Assert.Null(question);
    }
    
    [Fact]
    public void CanNotGoBeyondLastQuestion()
    {

        // ARRANGE
        SessionService sessionService = new();
        sessionService.Start("Test Session", [new Question(1, "What is your favorite color?"), new Question(2, "What is your favorite food?")]);
        sessionService.NextQuestion();
        sessionService.NextQuestion();

        // ACT
        var question = sessionService.NextQuestion();

        // ASSERT
        Assert.Null(question);
    }

    [Fact]
    public void CanSelectQuestions()
    {

        // ARRANGE
        SessionService sessionService = new();
        sessionService.Start("Test Session", [new Question(1, "What is your favorite color?"), new Question(2, "What is your favorite food?")]);
        
        // ACT
        var question = sessionService.NextQuestion();
        
        // ASSERT
        Assert.NotNull(question);
        Assert.Equal(1, question.Id);
    }

}