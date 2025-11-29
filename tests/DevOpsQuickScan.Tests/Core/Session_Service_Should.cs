using DevOpsQuickScan.Core;
using DevOpsQuickScan.Domain.Questions;

namespace DevOpsQuickScan.Tests.Core;

public class SessionServiceShould
{
    [Fact]
    public async Task Initialize_Session_Service()
    {
        // ARRANGE & ACT
        var sessionService = await InitializeSessionService();
        
        // ASSERT
        Assert.NotNull(sessionService);
        Assert.NotEmpty(sessionService.Questions);
        Assert.Equal(SessionState.NotStarted, sessionService.CurrentState);
    }

    [Fact]
    public async Task Accept_Participants()
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
    public async Task Not_Accept_Duplicate_Participants()
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
        sessionService.Join(participantId);
        
        // ASSERT
        Assert.Contains(participantId, sessionService.Participants);
        Assert.Equal(1, onParticipantJoinedCalled);
    }

    [Fact]
    public async Task Be_Able_To_Remove_Participant_From_Session()
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
    public async Task Not_Be_Able_To_Remove_A_Participant_Twice()
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
    public async Task Be_Able_To_Ask_Questions()
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
    public async Task Not_Be_Able_To_Ask_Question_Twice()
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
    public async Task Accept_Answers_From_Participants()
    {
        // ARRANGE
        var sessionService = await InitializeSessionService();
        QuestionWithAnswers? answeredQuestion = null;
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
    public async Task Not_Accept_Answer_From_Participant_Twice()
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

    [Fact]
    public async Task Be_Able_To_Reveal_Answers()
    {
        // ARRANGE
        var sessionService = await InitializeSessionService();
        var participantId = Guid.NewGuid().ToString();
        
        SessionState? sessionState = null;
        QuestionWithAnswers? revealedQuestion = null;
        sessionService.OnAnswersRevealed += (state, question) =>
        {
            sessionState = state;
            revealedQuestion = question;
        };
        sessionService.Join(participantId);
        sessionService.AskQuestion(1);
        sessionService.AnswerQuestion(participantId, 1, 2);

        // ACT
        sessionService.RevealQuestion(1); 

        // ASSERT
        Assert.Equal(sessionState, SessionState.RevealingAnswers);
        Assert.NotNull(revealedQuestion);
        Assert.Equal(1, revealedQuestion.Question.Id);
        Assert.True(sessionService.Questions.First(q => q.Id == 1).IsRevealed);
    }

    [Fact]
    public async Task Create_A_Question_With_The_Counted_Answers()
    {
        // ARRANGE
        var sessionService = await InitializeSessionService();
        var participantId = Guid.NewGuid().ToString();
        sessionService.Join(participantId);
        sessionService.AskQuestion(1);
        sessionService.AnswerQuestion(participantId, 1, 2);

        // ACT
        var questionWithAnswers = sessionService.QuestionWithAnswers(1);

        // ASSERT
        Assert.Equal(1, questionWithAnswers.Question.Id);
        Assert.Equal(0, questionWithAnswers.Answers.First(a => a.AnswerId == 1).NumberOfVotes);
        Assert.Equal(1, questionWithAnswers.Answers.First(a => a.AnswerId == 2).NumberOfVotes);
        Assert.Equal(0, questionWithAnswers.Answers.First(a => a.AnswerId == 3).NumberOfVotes);
        Assert.Equal(0, questionWithAnswers.Answers.First(a => a.AnswerId == 4).NumberOfVotes);
        Assert.Equal(0, questionWithAnswers.Answers.First(a => a.AnswerId == 5).NumberOfVotes);
    }

    [Fact]
    public async Task Be_Able_To_Reset_An_Active_Question()
    {
        // ARRANGE
        var sessionService = await InitializeSessionService();
        SessionState? sessionState = null;
        Question? currentQuestion = null;
        sessionService.OnQuestionAsked += (state, question) =>
        {
            sessionState = state;
            currentQuestion = question;
        };
        
        var participantId = Guid.NewGuid().ToString();
        sessionService.Join(participantId);
        sessionService.AskQuestion(1);
        sessionService.AnswerQuestion(participantId, 1, 2);
        sessionService.RevealQuestion(1);
        
        // ACT
        sessionService.ResetQuestion(1);
        
        // ASSERT
        Assert.False(sessionService.Questions.First(q => q.Id == 1).IsRevealed);
        
        var questionWithAnswers = sessionService.QuestionWithAnswers(1);
        Assert.DoesNotContain(questionWithAnswers.Answers, x => x.NumberOfVotes > 0);
        Assert.Equal(SessionState.NotStarted, sessionService.CurrentState);
        Assert.Null(sessionService.CurrentQuestion);
        Assert.Equal(SessionState.NotStarted, sessionState);
        Assert.Null(currentQuestion);
    }
    
    [Fact]
    public async Task Be_Able_To_Rename_An_Inactive_Question()
    {
        // ARRANGE
        var sessionService = await InitializeSessionService();
        SessionState? sessionState = null;
        Question? currentQuestion = null;
        sessionService.OnQuestionAsked += (state, question) =>
        {
            sessionState = state;
            currentQuestion = question;
        };
        
        var participantId = Guid.NewGuid().ToString();
        sessionService.Join(participantId);
        sessionService.AskQuestion(1);
        sessionService.AnswerQuestion(participantId, 1, 2);
        sessionService.RevealQuestion(1);
        sessionService.AskQuestion(2);
        
        // ACT
        sessionService.ResetQuestion(1);
        
        // ASSERT
        Assert.False(sessionService.Questions.First(q => q.Id == 1).IsRevealed);
        
        var questionWithAnswers = sessionService.QuestionWithAnswers(1);
        Assert.DoesNotContain(questionWithAnswers.Answers, x => x.NumberOfVotes > 0);
        Assert.Equal(SessionState.AnsweringQuestions, sessionService.CurrentState);
        Assert.NotNull(sessionService.CurrentQuestion);
        Assert.Equal(2, sessionService.CurrentQuestion.Id);
        Assert.Equal(SessionState.AnsweringQuestions, sessionState);
        Assert.NotNull(currentQuestion);
        Assert.Equal(2, currentQuestion.Id);
    }

    [Fact]
    public async Task Be_Able_To_Reset_The_Complete_Session()
    {
        // ARRANGE
        var sessionService = await InitializeSessionService();
        SessionState? sessionState = null;
        Question? currentQuestion = null;
        sessionService.OnQuestionAsked += (state, question) =>
        {
            sessionState = state;
            currentQuestion = question;
        };
        
        var participantId = Guid.NewGuid().ToString();
        sessionService.Join(participantId);
        sessionService.AskQuestion(1);
        sessionService.AnswerQuestion(participantId, 1, 2);
        sessionService.RevealQuestion(1);
        sessionService.AskQuestion(2);
        
        // ACT
        sessionService.Reset();
        
        // ASSERT
        Assert.Equal(SessionState.NotStarted, sessionService.CurrentState);
        Assert.Equal(SessionState.NotStarted, sessionState);
        
        Assert.Null(sessionService.CurrentQuestion);
        Assert.Null(currentQuestion);

        Assert.Empty(sessionService.Participants);
    }

    private async Task<SessionService> InitializeSessionService()
    {
        QuestionsService questionsService = new("Core");
        SessionService sessionService = new(questionsService, new ExportService());
        await sessionService.Initialize();
        return sessionService;
    }
}