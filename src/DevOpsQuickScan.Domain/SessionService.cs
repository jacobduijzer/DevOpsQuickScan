namespace DevOpsQuickScan.Domain;

public class SessionService(
    IQuestionRepository questions,
    ISessionRepository sessions)
{
    public int CurrentQuestionIndex = 0;
    public int NumberOfQuestions => _session!.Questions.Count;
        
    private Session? _session;
    
    
    public async Task<Guid> CreateSession(Guid facilitatorId, string sessionName)
    {
       var questionData = await questions.Get();
       _session = new Session(facilitatorId, sessionName, questionData.Questions);
       await sessions.Save(_session);
       return _session.Id;
    }
    
    public async Task Start()
    {
        _session!.Start();
        await sessions.Save(_session);
    }

    public Question? CurrentQuestion() => 
        _session!.Questions.ElementAtOrDefault(CurrentQuestionIndex);
    
    public Question? NextQuestion()
    {
        if (CurrentQuestionIndex >= _session!.Questions.Count - 1) 
            return default;
        
        CurrentQuestionIndex++;
        // check if it is already answered?
        return _session.Questions.ElementAtOrDefault(CurrentQuestionIndex);
    }

    public Question? PreviousQuestion()
    {
        if (CurrentQuestionIndex <= 0) 
            return default;
        
        CurrentQuestionIndex--;
        return _session!.Questions.ElementAtOrDefault(CurrentQuestionIndex);
    }
    
    private async Task<Session> Load(Guid sessionId) =>
        await sessions.Load(sessionId);
}