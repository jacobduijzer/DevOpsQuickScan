using DevOpsQuickScan.Domain;
using Moq;
using Xunit.Abstractions;

namespace DevOpsQuickScan.UnitTests.Domain;

public class SessionServiceTests_Storing_State(ITestOutputHelper outputHelper)
{
    [Fact]
    public async Task SessionServiceCanRestoreState()
    {
        // Arrange
        FakeSessionDataRepository fakeSessionDataRepository = new();
        var mockQuestionSender = new Mock<IQuestionSender>();
        var mockAnswersSender = new Mock<IAnswersSender>();
        SessionService sessionService = new(mockQuestionSender.Object, mockAnswersSender.Object, fakeSessionDataRepository, new XunitLogger<SessionService>(outputHelper));
        var sessionId = await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        await sessionService.NextQuestion();
        await sessionService.AskQuestion();
        
        // Act
        SessionService restoredSessionService = new(mockQuestionSender.Object, mockAnswersSender.Object, fakeSessionDataRepository, new XunitLogger<SessionService>(outputHelper));
        await restoredSessionService.Restore(sessionId);
        
        // Assert
        Assert.Multiple(
            () => Assert.IsType<SessionService>(restoredSessionService),
            () => Assert.Equal(sessionService.CurrentState, restoredSessionService.CurrentState),
            () => Assert.Equal(sessionService.SessionName, restoredSessionService.SessionName),
            () => Assert.Equal(sessionService.CurrentQuestionIndex, restoredSessionService.CurrentQuestionIndex)
        );
    }
    
    [Fact]
    public async Task SessionServiceCanRestoreSessionsWithAnsweredQuestionstate()
    {
        // Arrange
        FakeSessionDataRepository fakeSessionDataRepository = new();
        var mockQuestionSender = new Mock<IQuestionSender>();
        var mockAnswersSender = new Mock<IAnswersSender>();
        SessionService sessionService = new(mockQuestionSender.Object, mockAnswersSender.Object, fakeSessionDataRepository, new XunitLogger<SessionService>(outputHelper));
        var sessionCode = await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        await sessionService.NextQuestion();
        await sessionService.AskQuestion();
        var question = await sessionService.NextQuestion();
        await sessionService.AskQuestion();
        await sessionService.AddAnswer(new UserAnswer
        { 
            SessionCode = sessionCode,
            UserId = Guid.NewGuid(),
            QuestionId = question!.Id,
            AnswerId = question.Answers.First().Id
        });
        
        // Act
        SessionService restoredSessionService = new(mockQuestionSender.Object, mockAnswersSender.Object, fakeSessionDataRepository, new XunitLogger<SessionService>(outputHelper));
        await restoredSessionService.Restore(sessionCode);
        
        // Assert
        Assert.Multiple(
            () => Assert.IsType<SessionService>(restoredSessionService),
            () => Assert.Equal(sessionService.CurrentState, restoredSessionService.CurrentState),
            () => Assert.Equal(sessionService.SessionName, restoredSessionService.SessionName),
            () => Assert.Equal(sessionService.CurrentQuestionIndex, restoredSessionService.CurrentQuestionIndex),
            () => Assert.Equal(sessionService.NumberOfQuestions, restoredSessionService.NumberOfQuestions)
        );
    }
    
    [Fact]
    public async Task SessionServiceCanRestoreFinishedSessionState()
    {
        // Arrange
        FakeSessionDataRepository fakeSessionDataRepository = new();
        var mockQuestionSender = new Mock<IQuestionSender>();
        var mockAnswersSender = new Mock<IAnswersSender>();
        SessionService sessionService = new(mockQuestionSender.Object, mockAnswersSender.Object, fakeSessionDataRepository, new XunitLogger<SessionService>(outputHelper));
        var sessionId = await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        await sessionService.NextQuestion();
        await sessionService.AskQuestion();
        await sessionService.NextQuestion();
        await sessionService.AskQuestion();
        await sessionService.Finish();
        
        // Act
        SessionService restoredSessionService = new(mockQuestionSender.Object, mockAnswersSender.Object, fakeSessionDataRepository, new XunitLogger<SessionService>(outputHelper));
        await restoredSessionService.Restore(sessionId);
        
        // Assert
        Assert.Multiple(
            () => Assert.IsType<SessionService>(restoredSessionService),
            () => Assert.Equal(sessionService.CurrentState, restoredSessionService.CurrentState),
            () => Assert.Equal(sessionService.SessionName, restoredSessionService.SessionName),
            () => Assert.Equal(sessionService.CurrentQuestionIndex, restoredSessionService.CurrentQuestionIndex),
            () => Assert.Equal(sessionService.NumberOfQuestions, restoredSessionService.NumberOfQuestions)
        );
    }
}