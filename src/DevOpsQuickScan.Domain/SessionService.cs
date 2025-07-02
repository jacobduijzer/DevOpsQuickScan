using Microsoft.Extensions.Logging;
using Stateless;
using Stateless.Graph;

namespace DevOpsQuickScan.Domain;

public class SessionService(
    IQuestionSender questionSender,
    IAnswersSender answersSender,
    ISessionDataRepository sessionDataRepository, 
    ILogger<SessionService> logger)
{
    private readonly ISessionDataRepository? _sessionDataRepository = sessionDataRepository;
    private StateMachine<SessionState, SessionTrigger>? _sessionState;
    private SessionData? _sessionData;

    public SessionState CurrentState => _sessionState!.State;
    public string SessionName => _sessionData?.SessionName ?? string.Empty;
    public int CurrentQuestionIndex => _sessionData?.CurrentQuestionIndex ?? -1;
    public int NumberOfQuestions => _sessionData?.Questions?.Count ?? 0;

    public async Task<Guid> Start(string sessionName, List<Question?> questions)
    {
        _sessionData = new SessionData
        {
            SessionId = Guid.NewGuid(),
            SessionName = sessionName,
            Questions = questions,
            CurrentQuestionIndex = -1,
            CurrentState = SessionState.Initial,
            UserAnswers = new HashSet<UserAnswer>()
        };

        _sessionState = new StateMachine<SessionState, SessionTrigger>(_sessionData!.CurrentState);
        ConfigureStateMachine();

        await _sessionState.FireAsync(SessionTrigger.Start);
        
        return _sessionData.SessionId;
    }

    public async Task Restore(Guid sessionId)
    {
        _sessionData = await _sessionDataRepository!.Retrieve(sessionId);
        _sessionState = new StateMachine<SessionState, SessionTrigger>(_sessionData!.CurrentState);
        ConfigureStateMachine();
    }

    private void ConfigureStateMachine()
    {
        _sessionState!.Configure(SessionState.Initial)
            .PermitIf(SessionTrigger.Start, SessionState.Started,
                () => !string.IsNullOrEmpty(_sessionData!.SessionName) && _sessionData.Questions.Count > 0,
                "A session name must be set before starting and at least one question must be added.");

        _sessionState.Configure(SessionState.Started)
            .PermitIf(SessionTrigger.AskQuestion, SessionState.AwaitAnswers,
                () => _sessionData!.CurrentQuestionIndex > -1 && _sessionData.CurrentQuestionIndex < _sessionData.Questions.Count,
                "A question must be selected before asking.")
            .PermitIf(SessionTrigger.Finish, SessionState.Completed);

        _sessionState.Configure(SessionState.AwaitAnswers)
            .PermitIf(SessionTrigger.RevealAnswers, SessionState.AnswersRevealed)
            .PermitIf(SessionTrigger.Finish, SessionState.Completed)
            .PermitReentry(SessionTrigger.AskQuestion);

        _sessionState.Configure(SessionState.AnswersRevealed)
            .PermitIf(SessionTrigger.AskQuestion, SessionState.AwaitAnswers)
            .PermitIf(SessionTrigger.Finish, SessionState.Completed);

        _sessionState.OnTransitionedAsync(SaveState);
    }

    public string GetStateGraph() => MermaidGraph.Format(_sessionState!.GetInfo());

    public async Task AskQuestion()
    {
        if (_sessionData!.CurrentQuestionIndex > -1 && _sessionData.CurrentQuestionIndex < _sessionData.Questions.Count)
        {
            await questionSender.Send(_sessionData!.SessionId, _sessionData!.Questions[CurrentQuestionIndex]!);
            await _sessionState!.FireAsync(SessionTrigger.AskQuestion);
        }
        else throw new InvalidOperationException("A question must be selected before asking.");
    }

    public async Task RevealAnswers()
    {
        await answersSender.Send(_sessionData!.SessionId, _sessionData!.Questions[CurrentQuestionIndex]!, _sessionData!.GetAnswersCount());
        await _sessionState!.FireAsync(SessionTrigger.RevealAnswers);
    }

    public async Task Finish()
    {
        await _sessionState!.FireAsync(SessionTrigger.Finish);
    }

    public Question? NextQuestion()
    {
        if (CurrentQuestionIndex >= _sessionData!.Questions.Count - 1)
            return null;

        _sessionData!.CurrentQuestionIndex++;
        return _sessionData.Questions[CurrentQuestionIndex];
    }

    public Question? PreviousQuestion()
    {
        if (CurrentQuestionIndex <= 0)
            return null;

        _sessionData!.CurrentQuestionIndex--;
        return _sessionData.Questions[CurrentQuestionIndex];
    }

    public async Task AddAnswer(UserAnswer userAnswer)
    {
        _sessionData!.UserAnswers.Add(userAnswer);
        await SaveState();
    }

    private async Task SaveState(StateMachine<SessionState, SessionTrigger>.Transition? arg = null)
    {
        var sessionData = _sessionData! with { CurrentState = _sessionState!.State };
        await _sessionDataRepository!.Store(sessionData);
    }
}