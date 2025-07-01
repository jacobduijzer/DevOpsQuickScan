using DevOpsQuickScan.Domain;
using Xunit.Abstractions;

namespace DevOpsQuickScan.UnitTests.Domain;

public class SessionServiceTests_State_Transitions(ITestOutputHelper outputHelper)
{
    [Fact]
    public async Task CanCreateSessionGraph()
    {
        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);

        // ACT
        var sessionGraph = sessionService.GetStateGraph();

        // ASSERT
        Assert.NotNull(sessionGraph);
    }
    
    [Fact]
    public async Task CanNotTransitionWithoutSessionName()
    {
        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));

        // ACT & ASSERT
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await sessionService.Start("", []));
        Assert.Contains("A session name must be set before starting and at least one question must be added.",
            exception.Message);
    }

    [Fact]
    public async Task CanNotTransitionWithoutQuestions()
    {
        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));

        // ACT & ASSERT
        var exception =
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await sessionService.Start("", TestQuestionRepository.Questions!));
        Assert.Contains("A session name must be set before starting and at least one question must be added.",
            exception.Message);
    }

    [Fact]
    public async Task ASessionCanStartWhenNameAndQuestionsAreSet()
    {
        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));

        // ACT
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);

        // ASSERT
        Assert.Equal(SessionState.Started, sessionService.CurrentState);
    }

    [Fact]
    public async Task CanNotAskQuestionWithoutSettingCurrentQuestion()
    {
        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);

        // ACT & ASSERT
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(sessionService.AskQuestion);
        Assert.Contains("A question must be selected before asking.", exception.Message);
    }

    [Fact]
    public async Task CanRevealAnswersAfterAskingQuestion()
    {
        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        sessionService.NextQuestion();
        await sessionService.AskQuestion();

        // ACT
        await sessionService.RevealAnswers();

        // ASSERT
        Assert.Equal(SessionState.AnswersRevealed, sessionService.CurrentState);
    }

    [Fact]
    public async Task CanAnswerNewQuestionAfterRevealingAnswers()
    {
        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        sessionService.NextQuestion();
        await sessionService.AskQuestion();
        await sessionService.RevealAnswers();

        // ACT
        await sessionService.AskQuestion();

        // ASSERT
        Assert.Equal(SessionState.AwaitAnswers, sessionService.CurrentState);
    }

    [Fact]
    public async Task CanFinishSessionWithoutAskingQuestions()
    {
        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);

        // ACT
        await sessionService.Finish();

        // ASSERT
        Assert.Equal(SessionState.Completed, sessionService.CurrentState);
    }

    [Fact]
    public async Task CanFinishSessionWhenAwaitingAnswers()
    {
        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        sessionService.NextQuestion();
        await sessionService.AskQuestion();

        // ACT
        await sessionService.Finish();

        // ASSERT
        Assert.Equal(SessionState.Completed, sessionService.CurrentState);
    }

    [Fact]
    public async Task CanFinishSessionWhenRevealingAnswers()
    {
        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        sessionService.NextQuestion();
        await sessionService.AskQuestion();
        await sessionService.RevealAnswers();

        // ACT
        await sessionService.Finish();

        // ASSERT
        Assert.Equal(SessionState.Completed, sessionService.CurrentState);
    }
}