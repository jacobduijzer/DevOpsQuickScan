using DevOpsQuickScan.Domain.Sessions;
using DevOpsQuickScan.Infrastructure.Sessions;

namespace DevOpsQuickScan.Tests.Domain.Sessions;

public class SessionServiceTests
{
    [Fact]
    public async Task CanCreateSessionFile()
    {
        // ARRANGE
        IQuestionReader questionReader = new QuestionReader("Core");
        ISessionWriter sessionWriter = new SessionWriter("Core");
        SessionService sessionService = new SessionService(questionReader, new SessionReader(), sessionWriter);

        // ACT
        var sessionId = await sessionService.Create("Test Session", "testsession@example.com");

        // ASSERT
    }
}