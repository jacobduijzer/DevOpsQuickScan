using DevOpsQuickScan.Core;

namespace DevOpsQuickScan.Tests.Core;

public class SessionServiceTests
{
    [Fact]
    public async Task CanInitializeSessionService()
    {
        // ARRANGE & ACT
        var sessionService = await InitializeSessionService();
        
        // ASSERT
        Assert.NotNull(sessionService);
        Assert.NotEmpty(sessionService.Questions);
        Assert.Equal(SessionState.NotStarted, sessionService.CurrentState);
    }

    [Fact]
    public async Task CanJoinSession()
    {
        // ARRANGE
        var sessionService = await InitializeSessionService();
        var onParticipantJoinedCalled = 0;
        sessionService.OnParticipantJoined += () =>
        {
            onParticipantJoinedCalled++;
        };
        var participantId = Guid.NewGuid().ToString();
        
        // ACT
        sessionService.Join(participantId);
        
        // ASSERT
        Assert.Contains(participantId, sessionService.Participants);
        Assert.Equal(1, onParticipantJoinedCalled);
    }

    [Fact]
    public async Task CanRemoveUserFromSession()
    {
        // ARRANGE
        var sessionService = await InitializeSessionService();
        var onParticipantJoinedCalled = 0;
        sessionService.OnParticipantJoined += () =>
        {
            onParticipantJoinedCalled++;
        };
        var participantId = Guid.NewGuid().ToString();
        sessionService.Join(participantId);
        
        // ACT
        sessionService.Remove(participantId);
        
        // ASSERT
        Assert.DoesNotContain(participantId, sessionService.Participants); 
        Assert.Equal(2, onParticipantJoinedCalled);
    }
    
    [Fact]
    public async Task CannotRemoveUserFromSessionTwice()
    {
        // ARRANGE
        var sessionService = await InitializeSessionService();
        var onParticipantJoinedCalled = 0;
        sessionService.OnParticipantJoined += () =>
        {
            onParticipantJoinedCalled++;
        };
        var participantId = Guid.NewGuid().ToString();
        sessionService.Join(participantId);
        sessionService.Remove(participantId);
        
        // ACT
        sessionService.Remove(participantId);
        
        // ASSERT
        Assert.DoesNotContain(participantId, sessionService.Participants); 
        Assert.Equal(2, onParticipantJoinedCalled);
    }

    [Fact]
    public async Task CanAskQuestion()
    {
        // ARRANGE
        var sessionService = await InitializeSessionService();
        SessionState? sessionState = null;
        Question? askedQuestion = null;
        sessionService.OnQuestionAsked += (state, question) =>
        {
            sessionState = state;
            askedQuestion = question;
        };
        
        // ACT
        sessionService.AskQuestion(1);
        
        // ASSERT
        Assert.NotNull(sessionService.CurrentQuestion);
        Assert.Equal(1, sessionService.CurrentQuestion.Id);
        Assert.Equal(SessionState.AnsweringQuestions, sessionService.CurrentState);
        Assert.Equal(SessionState.AnsweringQuestions, sessionState);
        Assert.Equal(1, askedQuestion!.Id);
    }

    [Fact]
    public async Task CanNotAskQuestionAgain()
    {
        // ARRANGE
        var sessionService = await InitializeSessionService();
        var onQuestionAskedCalled = 0;
        sessionService.OnQuestionAsked += (state, question) =>
        {
            onQuestionAskedCalled++;
        };
        sessionService.AskQuestion(1);
        
        // ACT
        sessionService.AskQuestion(1);
        
        // ASSERT
        Assert.NotNull(sessionService.CurrentQuestion);
        Assert.Equal(1, sessionService.CurrentQuestion.Id);
        Assert.Equal(SessionState.AnsweringQuestions, sessionService.CurrentState);
        Assert.Equal(1, onQuestionAskedCalled);
    }

    [Fact]
    public async Task ParticipantCanAnswerQuestion()
    {
        // ARRANGE
        var sessionService = await InitializeSessionService();
        RevealedQuestion? answeredQuestion = null;
        sessionService.OnAnswerReceived += question =>
        {
            answeredQuestion = question;
        };
        var participantId = Guid.NewGuid().ToString();
        sessionService.AskQuestion(1);

        // ACT
        sessionService.AnswerQuestion(participantId, 1, 2);

        // ASSERT
        Assert.NotNull(answeredQuestion);
        Assert.Equal(1, answeredQuestion.Question.Id);
        Assert.Equal(5, answeredQuestion.Answers.Count);
        Assert.Equal(1, answeredQuestion.Answers.Count(a => a.AnswerId == 2));
        Assert.Equal(2, sessionService.GetAnswerId(participantId, 1));
    }

    [Fact]
    public async Task CanNotAnswerQuestionAgain()
    {
        // ARRANGE
        var sessionService = await InitializeSessionService();
        int onAnsweredCalled = 0;
        sessionService.OnAnswerReceived += question =>
        {
            onAnsweredCalled++;
        };
        var participantId = Guid.NewGuid().ToString();
        sessionService.AskQuestion(1);

        // ACT
        sessionService.AnswerQuestion(participantId, 1, 2);
        sessionService.AnswerQuestion(participantId, 1, 2);

        // ASSERT
        Assert.Equal(1, onAnsweredCalled); 
    }

    private async Task<SessionService> InitializeSessionService()
    {
        QuestionsService questionsService = new("Core");
        SessionService sessionService = new(questionsService);
        await sessionService.Initialize();
        return sessionService;
    }
}