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
        
        Question question = new (1, "What is your favorite color", "https://google.com?q=favorite%20color", [
            new Answer(1, "Red"), 
            new Answer(2, "Blue"), 
            new Answer(3, "Green"),
            new Answer(4, "Cheese")]);
            
        ISessionRepository sessionRepository = new SessionRepository(storageAccountName, storageAccountKey, sessionsContainerName);
        Session session = new(Guid.NewGuid(), "Test Session", [question]);

        // ACT
        session.Start();
        session.SelectQuestion(question);
        session.AnswerQuestion(Guid.NewGuid(), 1, 3);
        await sessionRepository.Save(session);

        // ASSERT
        outputWriter.WriteLine(session.Id.ToString());
    }

    [Fact]
    public async Task CanReadSessionData()
    {
        // ARRANGE
        var sessionId = Guid.Parse("568791c3-c66b-44be-b4e4-5ac82926e36c");

        var storageAccountName = "devopsquickscanwu33ih6we";
        var storageAccountKey = "oqeV4hoWJspdBNIGGaht2ONzGpyKuyirYv6dZafWSavg5NjWsVKP3JKtGdDcpvQUBQmspbL5S82d+AStVj40hg==";
        var sessionsContainerName = "https://devopsquickscanwu33ih6we.blob.core.windows.net/sessiondata";
            
        ISessionRepository sessionRepository = new SessionRepository(storageAccountName, storageAccountKey, sessionsContainerName);

        // ACT
        var session = await sessionRepository.Load(sessionId);
        
        // ASSERT
        Assert.NotNull(session);
        Assert.Equal(sessionId, session.Id);
        Assert.Equal("Test Session", session.Name);
        Assert.Equal(SessionState.CollectingAnswers, session.CurrentState);
        Assert.Equal("What is your favorite color", session.CurrentQuestion?.Text);
    }
}