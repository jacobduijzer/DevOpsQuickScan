using DevOpsQuickScan.Domain;
using DevOpsQuickScan.UnitTests.Stubs;
using Moq;

namespace DevOpsQuickScan.UnitTests.Domain;

public class SessionServiceTests
{
    [Fact]
    public async Task FacilitatorCanCreateNewSession()
    {
        // ARRANGE
        var mockSessions = new Mock<ISessionRepository>();
        SessionService sessionService = new(new QuestionRepositoryStub(), mockSessions.Object);
        var facilitatorId = Guid.NewGuid();
        var sessionName = "Test Session";
    
        // ACT
        var sessionId = await sessionService.CreateSession(facilitatorId, sessionName);
    
        // ASSERT
        Assert.NotEqual(Guid.Empty, sessionId);
        mockSessions.Verify(x => x.Save(It.Is<Session>(session => 
            session.Id == sessionId &&
            session.FacilitatorId == facilitatorId && 
            session.Name == sessionName &&
            session.CurrentState == SessionState.NotStarted
            )), Times.Once());
    }

    [Fact]
    public async Task FacilitatorCanStartASession()
    {
        // ARRANGE
        var questionsStub = new QuestionRepositoryStub();
        var questionData = await questionsStub.Get();
        var mockSessions = new Mock<ISessionRepository>();
        var facilitatorId = Guid.NewGuid();
        var sessionName = "Test Session";
        
        SessionService sessionService = new(questionsStub, mockSessions.Object);
        var sessionId = await sessionService.CreateSession(facilitatorId, sessionName);

        mockSessions.Setup(x => x.Load(sessionId)).ReturnsAsync(
            new Session(sessionId, facilitatorId, sessionName, SessionState.NotStarted, questionData.Questions, null, new HashSet<QuestionAnswer>()));
            
        // ACT
        await sessionService.Start();

        // ASSERT
        Assert.NotEqual(Guid.Empty, sessionId);
        mockSessions.Verify(x => x.Save(It.Is<Session>(session =>
            session.Id == sessionId &&
            session.FacilitatorId == facilitatorId &&
            session.Name == sessionName &&
            session.CurrentState == SessionState.QuestionPending
        )), Times.Exactly(2));
    }
    
    [Fact]
    public async Task NextQuestionMovesToNextWhenNotAtEnd()
    {
        // ARRANGE
        var service = new SessionService(new QuestionRepositoryStub(), new SessionRepositoryStub());
        await service.CreateSession(Guid.NewGuid(), "Test");

        // ACT
        var first = service.CurrentQuestion();
        var next = service.NextQuestion();

        // ASSERT
        Assert.NotEqual(first!.Id, next!.Id);
    }
    
    [Fact]
    public async Task PreviousQuestionMovesToPreviousWhenNotAtStart()
    {
        // ARRANGE 
        var service = new SessionService(new QuestionRepositoryStub(), new SessionRepositoryStub());
        await service.CreateSession(Guid.NewGuid(), "Test");
        await service.Start();

        // ACT
        service.NextQuestion();
        var first = service.NextQuestion();
        var second = service.PreviousQuestion();

        // ASSERT
        Assert.NotEqual(first!.Id, second!.Id);
    }
    
    [Fact]
    public async Task PreviousQuestionReturnsNullAtStart()
    {
        // Arrange
        var service = new SessionService(new QuestionRepositoryStub(), new SessionRepositoryStub());
        await service.CreateSession(Guid.NewGuid(), "Test");
        await service.Start();

        // Act
        var result = service.PreviousQuestion();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task NextQuestion_ReturnsNull_AtEnd()
    {
        // Arrange
        var service = new SessionService(new QuestionRepositoryStub(), new SessionRepositoryStub());
        await service.CreateSession(Guid.NewGuid(), "Test");
        // Move to last question
        while (service.NextQuestion() != null) { }

        // ACT
        var result = service.NextQuestion();

        // Assert
        Assert.Null(result);
    }
}