using DevOpsQuickScan.Domain;
using DevOpsQuickScan.Infrastructure;
using Xunit.Abstractions;

namespace DevOpsQuickScan.UnitTests.Infrastructure;

public class SessionRepositoryTests(ITestOutputHelper outputWriter)
{
    [Fact]
    public async Task CanWriteSessionData()
    {
        // ARRANGE
        var storageAccountName = "devopsquickscanwu33ih6we";
        var storageAccountKey = "oqeV4hoWJspdBNIGGaht2ONzGpyKuyirYv6dZafWSavg5NjWsVKP3JKtGdDcpvQUBQmspbL5S82d+AStVj40hg==";
        var sessionsContainerName = "https://devopsquickscanwu33ih6we.blob.core.windows.net/sessiondata";
            
        ISessionRepository sessionRepository = new SessionRepository(storageAccountName, storageAccountKey, sessionsContainerName);
        Session session = new(Guid.NewGuid(), "Test Session");

        // ACT
        session.Start();
        session.SelectQuestion(new Question("What is your favorite color?", [
            new Answer("Red"), 
            new Answer("Blue"), 
            new Answer("Green"),
            new Answer("Cheese")]));
        await sessionRepository.Save(session);

        // ASSERT
        outputWriter.WriteLine(session.Id.ToString());
    }

    [Fact]
    public async Task CanReadSessionData()
    {
        // ARRANGE
        var sessionId = Guid.Parse("69f2b54b-ae4c-4f41-a3d4-3ec70790c1e2");

        var storageAccountName = "devopsquickscanwu33ih6we";
        var storageAccountKey = "oqeV4hoWJspdBNIGGaht2ONzGpyKuyirYv6dZafWSavg5NjWsVKP3JKtGdDcpvQUBQmspbL5S82d+AStVj40hg==";
        var sessionsContainerName = "https://devopsquickscanwu33ih6we.blob.core.windows.net/sessiondata";
            
        ISessionRepository sessionRepository = new SessionRepository(storageAccountName, storageAccountKey, sessionsContainerName);

        // ACT
        var session = await sessionRepository.Load(sessionId);
        
        // ASSERT
        Assert.NotNull(session);
        Assert.Equal(sessionId, session.Id);
        Assert.Equal("Test Session", session.SessionName);
        Assert.Equal(SessionState.CollectingAnswers, session.CurrentState);
        Assert.Equal("What is your favorite color?", session.CurrentQuestion?.Text);
    }
}