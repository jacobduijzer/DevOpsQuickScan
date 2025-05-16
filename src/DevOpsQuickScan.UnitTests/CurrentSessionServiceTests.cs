using DevOpsQuickScan.Web.Sessions;
using DevOpsQuickScan.Web.Surveys;
using Moq;

namespace DevOpsQuickScan.UnitTests;

public class CurrentSessionServiceTests
{
    [Fact]
    public async Task CanStartSessionWithName()
    {
        // ARRANGE
        var sessionService = CreateSessionService();

        // ACT
        var sessionId = await sessionService.Start("test", "test", "survey-01.json");

        // ASSERT
        Assert.NotEmpty(sessionId);
    }

    [Fact]
    public async Task CanSendNextQuestions()
    {
        // // ARRANGE
        // var sessionService = CreateSessionService();
        // await sessionService.Start("test", "test", "survey-01.json");
        // Question? nextQuestion = null;
        // sessionService.OnNewQuestion += question => nextQuestion = question;
        //
        // // ACT
        // sessionService.NextQuestion();
        //
        // // ASSERT
        // Assert.NotNull(nextQuestion);
        // Assert.Equal("How frequently does your team deploy code to production or end users?", nextQuestion.Text);
    }
    
    [Fact]
    public async Task CanSendPreviousQuestions()
    {
        // // ARRANGE
        // var sessionService = CreateSessionService();
        // await sessionService.Start("test", "test", "survey-01.json");
        // Question? previousQuestion = null;
        // sessionService.OnNewQuestion += question => previousQuestion = question;
        // sessionService.NextQuestion();
        // sessionService.NextQuestion();
        //
        // // ACT
        // sessionService.PreviousQuestion();
        //
        // // ASSERT
        // Assert.NotNull(previousQuestion);
        // Assert.Equal("How long does it typically take to go from code committed to code running in production?", previousQuestion.Text);
    }

    [Fact]
    public async Task CanAddParticipantToSession()
    {
        // ARRANGE
        var mockHubConnectionWrapper = new Mock<IHubConnectionWrapper>();
        var sessionStore = new InMemorySessionStore();
        var surveyReader = new SurveyReader();
        var sessionService = new CurrentSessionService(mockHubConnectionWrapper.Object, sessionStore, surveyReader);
        
        var sessionId = await sessionService.Start("test", "test", "survey-01.json");
        Participant participant = new(sessionId, "John Doe");
        
        // ACT
        mockHubConnectionWrapper.Raise(m => m.OnParticipantJoined += null, participant);

        // ASSERT
        Assert.NotEmpty(sessionService.Participants); 
        Assert.Contains(participant, sessionService.Participants);
    }
    
    [Fact]
    public async Task ParticipantCanOnlJoinOnce()
    {
        // ARRANGE
        var mockHubConnectionWrapper = new Mock<IHubConnectionWrapper>();
        var sessionStore = new InMemorySessionStore();
        var surveyReader = new SurveyReader();
        var sessionService = new CurrentSessionService(mockHubConnectionWrapper.Object, sessionStore, surveyReader);
        
        var sessionId = await sessionService.Start("test", "test", "survey-01.json");
        Participant participant = new(sessionId, "John Doe");
        
        // ACT
        mockHubConnectionWrapper.Raise(m => m.OnParticipantJoined += null, participant);
        mockHubConnectionWrapper.Raise(m => m.OnParticipantJoined += null, participant);

        // ASSERT
        Assert.NotEmpty(sessionService.Participants); 
        Assert.Contains(participant, sessionService.Participants);
        Assert.Single(sessionService.Participants);
    }
    
    [Fact]
    public async Task ParticipantCanAnswerQuestion()
    {
        // // ARRANGE
        // var mockHubConnectionWrapper = new Mock<IHubConnectionWrapper>();
        // var sessionStore = new InMemorySessionStore();
        // var surveyReader = new SurveyReader();
        // var sessionService = new SessionService(mockHubConnectionWrapper.Object, sessionStore, surveyReader);
        //
        // var sessionId = await sessionService.Start("test", "test", "survey-01.json");
        // Participant participant = new(sessionId, "John Doe");
        // mockHubConnectionWrapper.Raise(m => m.OnParticipantJoined += null, participant);
        //
        // Question? previousQuestion = null;
        // sessionService.OnNewQuestion += question => previousQuestion = question;
        // sessionService.NextQuestion();
        // ParticipantAnswer participantAnswer = new(sessionId, "John Doe", previousQuestion.Id, previousQuestion.Answers[0].Id);
        //
        // // ACT
        // mockHubConnectionWrapper.Raise(m => m.OnNewAnswer += null, participantAnswer);
        //
        // // ASSERT
        // Assert.NotEmpty(sessionService.Answers); 
        // Assert.Contains(participantAnswer, sessionService.Answers);
    }
    
    [Fact]
    public async Task ParticipantCanNotAnswerQuestionTwice()
    {
        // // ARRANGE
        // var mockHubConnectionWrapper = new Mock<IHubConnectionWrapper>();
        // var sessionStore = new InMemorySessionStore();
        // var surveyReader = new SurveyReader();
        // var sessionService = new SessionService(mockHubConnectionWrapper.Object, sessionStore, surveyReader);
        //
        // var sessionId = await sessionService.Start("test", "test", "survey-01.json");
        // Participant participant = new(sessionId, "John Doe");
        // mockHubConnectionWrapper.Raise(m => m.OnParticipantJoined += null, participant);
        //
        // Question? previousQuestion = null;
        // sessionService.OnNewQuestion += question => previousQuestion = question;
        // sessionService.NextQuestion();
        // ParticipantAnswer participantAnswer = new(sessionId, "John Doe", previousQuestion.Id, previousQuestion.Answers[0].Id);
        //
        // // ACT
        // mockHubConnectionWrapper.Raise(m => m.OnNewAnswer += null, participantAnswer);
        // mockHubConnectionWrapper.Raise(m => m.OnNewAnswer += null, participantAnswer);
        //
        // // ASSERT
        // Assert.NotEmpty(sessionService.Answers); 
        // Assert.Contains(participantAnswer, sessionService.Answers);
        // Assert.Single(sessionService.Answers);
    }

    private CurrentSessionService CreateSessionService()
    {
        var mockHubConnectionWrapper = new Mock<IHubConnectionWrapper>();
        var sessionStore = new InMemorySessionStore();
        var surveyReader = new SurveyReader();
        return new CurrentSessionService(mockHubConnectionWrapper.Object, sessionStore, surveyReader);
    }
}