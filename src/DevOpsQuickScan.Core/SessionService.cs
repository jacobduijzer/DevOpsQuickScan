namespace DevOpsQuickScan.Core;

public class SessionService
{
    public event Action? OnQuestionChanged;

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

    public Dictionary<string, int> Answers { get; set; } = new(); 
}