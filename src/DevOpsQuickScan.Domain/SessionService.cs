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

    public async Task AskQuestion(Question question)
    {
        _session!.SelectQuestion(question);
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