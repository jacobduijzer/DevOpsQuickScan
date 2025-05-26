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
        var mockSessionEventsHandler = new Mock<ICommunicationEvents>();
        var communicationService = new CommunicationService(mockSessionEventsHandler.Object);
        SessionService sessionService = new(new QuestionRepositoryStub(), mockSessions.Object, communicationService);
        var facilitatorId = Guid.NewGuid();
        var sessionName = "Test Session";
    
        // ACT
        var sessionId = await sessionService.CreateSession(facilitatorId, sessionName, new Uri("/hub/vote"));
    
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
        var mockSessionEventsHandler = new Mock<ICommunicationEvents>();
        var communicationService = new CommunicationService(mockSessionEventsHandler.Object);
        var facilitatorId = Guid.NewGuid();
        var sessionName = "Test Session";
        
        SessionService sessionService = new(questionsStub, mockSessions.Object, communicationService);
        var sessionId = await sessionService.CreateSession(facilitatorId, sessionName, new Uri("/hub/vote"));

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
    public async Task ParticipantCanJoinASession()
    {
        // ARRANGE
        var questionsStub = new QuestionRepositoryStub();
        var questionData = await questionsStub.Get();
        var mockSessions = new Mock<ISessionRepository>();
        var sessionEventsHandler = new CommunicationEventsHandlerStub();
        var communicationService = new CommunicationService(sessionEventsHandler);
        
        var facilitatorId = Guid.NewGuid();
        var sessionName = "Test Session";

        var fired = false;
        
        SessionService sessionService = new(questionsStub, mockSessions.Object, communicationService);
        sessionService.OnParticipantJoined += participant =>
        {
            fired = true;
        };
        
        var sessionId = await sessionService.CreateSession(facilitatorId, sessionName, new Uri("/hub/vote"));

        mockSessions.Setup(x => x.Load(sessionId)).ReturnsAsync(
            new Session(sessionId, facilitatorId, sessionName, SessionState.NotStarted, questionData.Questions, null, new HashSet<QuestionAnswer>()));
        
        // ACT
        await sessionEventsHandler.Join(sessionId, "Jacob");

        // ASSERT
        Assert.True(fired);
    }
    
    [Fact]
    public async Task NextQuestionMovesToNextWhenNotAtEnd()
    {
        // ARRANGE
        var communicationService = new CommunicationService(new Mock<ICommunicationEvents>().Object);
        var service = new SessionService(new QuestionRepositoryStub(), new SessionRepositoryStub(), communicationService);
        await service.CreateSession(Guid.NewGuid(), "Test", new Uri("/hub/vote"));

        // ACT
        var first = service.CurrentQuestion();
        var next = service.NextQuestion();

        // ASSERT
        Assert.NotEqual(first!.Question.Id, next!.Question.Id);
    }
    
    [Fact]
    public async Task PreviousQuestionMovesToPreviousWhenNotAtStart()
    {
        // ARRANGE 
        var communicationService = new CommunicationService(new Mock<ICommunicationEvents>().Object);
        var service = new SessionService(new QuestionRepositoryStub(), new SessionRepositoryStub(), communicationService);
        await service.CreateSession(Guid.NewGuid(), "Test", new Uri("/hub/vote"));
        await service.Start();

        // ACT
        service.NextQuestion();
        var first = service.NextQuestion();
        var second = service.PreviousQuestion();

        // ASSERT
        Assert.NotEqual(first!.Question.Id, second!.Question.Id);
    }
    
    [Fact]
    public async Task PreviousQuestionReturnsNullAtStart()
    {
        // Arrange
        var communicationService = new CommunicationService(new Mock<ICommunicationEvents>().Object);
        var service = new SessionService(new QuestionRepositoryStub(), new SessionRepositoryStub(), communicationService);
        await service.CreateSession(Guid.NewGuid(), "Test", new Uri("/hub/vote"));
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
        var communicationService = new CommunicationService(new Mock<ICommunicationEvents>().Object);
        var service = new SessionService(new QuestionRepositoryStub(), new SessionRepositoryStub(), communicationService);
        await service.CreateSession(Guid.NewGuid(), "Test", new Uri("/hub/vote"));
        // Move to last question
        while (service.NextQuestion() != null) { }

        // ACT
        var result = service.NextQuestion();

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task CanAnswerQuestion()
    {
        // ARRANGE
        var communicationService = new CommunicationService(new Mock<ICommunicationEvents>().Object);
        var service = new SessionService(new QuestionRepositoryStub(), new SessionRepositoryStub(), communicationService);
        await service.CreateSession(Guid.NewGuid(), "Test", new Uri("/hub/vote"));
        await service.Start();
        var question = service.CurrentQuestion()!;
        await service.AskQuestion(question.Question.Id);
        
        // ACT
        await service.AnswerQuestion(Guid.NewGuid(), question.Question.Id, question.Question.Answers.First().Id);

        // ASSERT
        var questionWithAnswers = service.CurrentQuestion();
        Assert.NotNull(questionWithAnswers);
        Assert.Single(questionWithAnswers.Answers!);
    }
}