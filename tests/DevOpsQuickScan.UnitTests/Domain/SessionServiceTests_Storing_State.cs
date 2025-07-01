using System.Text.Json;
using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.UnitTests.Domain;

public class SessionServiceTests_Storing_State
{
    [Fact]
    public void SessionServiceCanStoreState()
    {
        // Arrange
        SessionService sessionService = new();
        sessionService.Start("Test Session", [new Question(1, "What is your favorite color?"), new Question(2, "What is your favorite food?")]);
        var json = JsonSerializer.Serialize(sessionService);

        // Act
        var restoredSessionService = JsonSerializer.Deserialize<SessionService>(json);

        // Assert
        Assert.IsType<SessionService>(restoredSessionService);
        Assert.Equal(sessionService.CurrentState, restoredSessionService.CurrentState);
        Assert.Equal(sessionService.SessionName, restoredSessionService.SessionName);
        // Assert.Equal(sessionService.Questions.Count, restoredSessionService.Questions.Count);
    }
}