using DevOpsQuickScan.Domain;
using Xunit.Abstractions;

namespace DevOpsQuickScan.UnitTests.Domain;

public class SessionServiceTests_Storing_State(ITestOutputHelper outputHelper)
{
    [Fact]
    public async Task SessionServiceCanRestoreState()
    {
        // Arrange
        FakeSessionDataRepository fakeSessionDataRepository = new();
        SessionService sessionService = new(fakeSessionDataRepository, new XunitLogger<SessionService>(outputHelper));
        var sessionId = await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        sessionService.NextQuestion();
        await sessionService.AskQuestion();
        
        // Act
        SessionService restoredSessionService = new(fakeSessionDataRepository, new XunitLogger<SessionService>(outputHelper));
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
    public async Task SessionServiceCanRestoreFinishedSessionState()
    {
        // Arrange
        FakeSessionDataRepository fakeSessionDataRepository = new();
        SessionService sessionService = new(fakeSessionDataRepository, new XunitLogger<SessionService>(outputHelper));
        var sessionId = await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        sessionService.NextQuestion();
        await sessionService.AskQuestion();
        sessionService.NextQuestion();
        await sessionService.AskQuestion();
        await sessionService.Finish();
        
        // Act
        SessionService restoredSessionService = new(fakeSessionDataRepository, new XunitLogger<SessionService>(outputHelper));
        await restoredSessionService.Restore(sessionId);
        
        // Assert
        Assert.Multiple(
            () => Assert.IsType<SessionService>(restoredSessionService),
            () => Assert.Equal(sessionService.CurrentState, restoredSessionService.CurrentState),
            () => Assert.Equal(sessionService.SessionName, restoredSessionService.SessionName),
            () => Assert.Equal(sessionService.CurrentQuestionIndex, restoredSessionService.CurrentQuestionIndex)
        );
    }
}