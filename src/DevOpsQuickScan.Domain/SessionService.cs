using System.Text.Json.Serialization;
using Stateless;

namespace DevOpsQuickScan.Domain;

public class SessionService
{
    private StateMachine<SessionState, SessionTrigger> _sessionState;
    [JsonInclude]
    private List<Question?> _questions = new ();

    public SessionState CurrentState => _sessionState.State;
    
    [JsonInclude]
    public string SessionName = string.Empty;
    public int CurrentQuestionIndex = -1;
    public int NumberOfQuestions => _questions.Count;

    public SessionService()
    {
        _sessionState = new StateMachine<SessionState, SessionTrigger>(SessionState.Initial);
        ConfigureStateMachine();
    }

    [JsonConstructor]
    public SessionService(string sessionName, SessionState currentState, List<Question?> _questions)
    {
        SessionName = sessionName;
        _sessionState = new StateMachine<SessionState, SessionTrigger>(currentState);
        _questions = _questions ?? new List<Question?>();
       
        ConfigureStateMachine();
    }

    private void ConfigureStateMachine()
    {
        _sessionState.Configure(SessionState.Initial)
            .PermitIf(SessionTrigger.Start, SessionState.Started,
                () => !string.IsNullOrEmpty(SessionName) && _questions.Count > 0,
                "A session name must be set before starting and at least one question must be added.")
            .OnEntry(async void () => await SaveState())
            .OnExit(async void () => await SaveState());

        _sessionState.Configure(SessionState.Started)
            .PermitIf(SessionTrigger.AskQuestion, SessionState.AwaitAnswers, () => CurrentQuestionIndex > -1 && CurrentQuestionIndex < _questions.Count,
                "A question must be selected before asking.")
            .PermitIf(SessionTrigger.Finish, SessionState.Completed);

        _sessionState.Configure(SessionState.AwaitAnswers)
            .PermitIf(SessionTrigger.RevealAnswers, SessionState.AnswersRevealed)
            .PermitIf(SessionTrigger.Finish, SessionState.Completed);

        _sessionState.Configure(SessionState.AnswersRevealed)
            .PermitIf(SessionTrigger.AskQuestion, SessionState.AwaitAnswers)
            .PermitIf(SessionTrigger.Finish, SessionState.Completed);
    }

    public void Start(string sessionName, List<Question?> questions)
    {
        SessionName = sessionName;
        _questions = questions;
        _sessionState.Fire(SessionTrigger.Start);
    }

    public void AskQuestion()
    {
        _sessionState.Fire(SessionTrigger.AskQuestion);
    }

    public void RevealAnswers()
    {
        _sessionState.Fire(SessionTrigger.RevealAnswers);
    }

    public void Finish()
    {
        _sessionState.Fire(SessionTrigger.Finish);
    }
    
    public Question? NextQuestion()
    {
        if (CurrentQuestionIndex >= _questions.Count - 1)
            return default;

        CurrentQuestionIndex++;
        return _questions[CurrentQuestionIndex];
    }

    public Question? PreviousQuestion()
    {
        if (CurrentQuestionIndex <= 0)
            return default;

        CurrentQuestionIndex--;
        return _questions[CurrentQuestionIndex];
    }

    private async Task SaveState()
    {
        // TODO
        await Task.CompletedTask;
    }
}