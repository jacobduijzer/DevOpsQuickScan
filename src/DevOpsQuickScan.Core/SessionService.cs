namespace DevOpsQuickScan.Core;

public class SessionService
{
    public event Action? OnQuestionChanged;
    public event Action? OnAnswerReceived;

    private Question? _currentQuestion;
    public Question? CurrentQuestion
    {
        get => _currentQuestion;
        set
        {
            _currentQuestion = value;
            OnQuestionChanged?.Invoke(); 
        }
    }

    public List<AnswerSubmission> Submissions { get; set; } = new();

    public bool HasAnsweredCurrentQuestion(string userId) =>
       Submissions.Any(x => x.UserId == userId && x.QuestionId == _currentQuestion?.Id); 
    
    public void AnswerQuestion(string userId, int answerId)
    {
        // TODO: make sure there are no duplicates
        Submissions.Add(new AnswerSubmission(userId, _currentQuestion!.Id, answerId));
        OnAnswerReceived?.Invoke();
    }

    public int GetAnswer(string userId) =>
        (int)Submissions.FirstOrDefault(x => x.UserId == userId && x.QuestionId == _currentQuestion?.Id)?.AnswerId!;
}