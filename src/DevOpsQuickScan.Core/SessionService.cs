namespace DevOpsQuickScan.Core;

public class SessionService
{
    public event Action<Question>? OnQuestionChanged;
    public event Action? OnAnswerReceived;
    public event Action? OnAnswersRevealed;

    private Question? _currentQuestion;
    public Question? CurrentQuestion
    {
        get => _currentQuestion;
        set
        {
            _currentQuestion = value;
            OnQuestionChanged?.Invoke(_currentQuestion!); 
        }
    }

    public List<AnswerSubmission> Submissions { get; set; } = new();

    public bool HasAnsweredCurrentQuestion(string userId) =>
       Submissions.Any(x => x.UserId == userId && x.QuestionId == _currentQuestion?.Id); 
    
    public void AnswerQuestion(string userId, int answerId)
    {
        if (CurrentQuestion!.IsRevealed)
            return;
        
        // TODO: make sure there are no duplicates
        Submissions.Add(new AnswerSubmission(userId, _currentQuestion!.Id, answerId));
        OnAnswerReceived?.Invoke();
    }

    public int GetAnswer(string userId) =>
        (int)Submissions.FirstOrDefault(x => x.UserId == userId && x.QuestionId == _currentQuestion?.Id)?.AnswerId!;

    public void RevealQuestion(int questionId)
    {
        OnAnswersRevealed?.Invoke();
    }
    
    public void ResetQuestion(int questionId) =>
        Submissions.RemoveAll(x => x.QuestionId == questionId);
}