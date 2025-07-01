using DevOpsQuickScan.Domain;
using Xunit.Abstractions;

namespace DevOpsQuickScan.UnitTests.Domain;

public class SessionServiceTests_Questions(ITestOutputHelper outputHelper)
{
    [Fact]
    public async Task CanNotGoToPreviousQuestionWhenStarting()
    {
        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);

        // ACT
        var question = sessionService.PreviousQuestion();

        // ASSERT
        Assert.Null(question);
    }
    
    [Fact]
    public async Task CanNotGoBeyondLastQuestion()
    {

        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        sessionService.NextQuestion();
        sessionService.NextQuestion();

        // ACT
        var question = sessionService.NextQuestion();

        // ASSERT
        Assert.Null(question);
    }

    [Fact]
    public async Task CanSelectQuestions()
    {

        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        
        // ACT
        var question = sessionService.NextQuestion();
        
        // ASSERT
        Assert.NotNull(question);
        Assert.Equal(1, question.Id);
    }

}