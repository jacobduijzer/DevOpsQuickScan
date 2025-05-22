namespace DevOpsQuickScan.Domain;

// TODO: IAsyncDisposable?
public class SessionService(
    IQuestionRepository questions,
    ISessionRepository sessions,
    CommunicationService communication)
{
    public event Action<Participant>? OnParticipantJoined;

    public int CurrentQuestionIndex = 0;
    public int NumberOfQuestions => _session!.Questions.Count;

    private Session? _session;

    public async Task<Guid> CreateSession(Guid facilitatorId, string sessionName, Uri hubUrl)
    {
        communication.OnParticipantJoined += participant => 
            OnParticipantJoined?.Invoke(participant);;
        
        var questionData = await questions.Get();
        _session = new Session(facilitatorId, sessionName, questionData.Questions);
        await sessions.Save(_session);
        await communication.Start(hubUrl);
        await communication.Join(_session.Id, "Facilitator");

        return _session.Id;
    }

    // TODO: probably move to Create
    public async Task Start()
    {
        _session!.Start();
        await sessions.Save(_session);
    }
    
    public async Task Join(Uri hubUri, Guid sessionId, string displayName)
    {
        await communication.Start(hubUri);
        await communication.Join(sessionId, displayName);
    }
    
    public async Task<string> SessionName(Guid sessionId)
    {
        _session = await Load(sessionId);
        if (_session == null)
            throw new InvalidOperationException($"Session with ID {sessionId} not found.");

        return _session.Name;
    }

    public async Task AskQuestion(int questionId)
    {
        _session!.SelectQuestion(_session.Questions.First(q => q.Id == questionId));
        await sessions.Save(_session);
    }

    public async Task AnswerQuestion(Guid participantId, int questionId, int answerId)
    {
        _session!.AnswerQuestion(participantId, questionId, answerId);
        await sessions.Save(_session);
    }

    public QuestionWithAnswers? CurrentQuestion() =>
        QuestionWithAnswers(CurrentQuestionIndex);

    public QuestionWithAnswers? NextQuestion()
    {
        if (CurrentQuestionIndex >= _session!.Questions.Count - 1)
            return default;

        CurrentQuestionIndex++;
        return QuestionWithAnswers(CurrentQuestionIndex);
    }

    public QuestionWithAnswers? PreviousQuestion()
    {
        if (CurrentQuestionIndex <= 0)
            return default;

        CurrentQuestionIndex--;
        return QuestionWithAnswers(CurrentQuestionIndex);
    }

    private async Task<Session> Load(Guid sessionId) =>
        await sessions.Load(sessionId);

    private QuestionWithAnswers? QuestionWithAnswers(int index)
    {
        var question = _session!.Questions.ElementAtOrDefault(index)!;
        var answers = _session!.Answers.Where(x => x.QuestionId == question.Id).ToList();
        return new QuestionWithAnswers(question, answers.Any() ? answers : null);
    }
       
}