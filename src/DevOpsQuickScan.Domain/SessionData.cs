namespace DevOpsQuickScan.Domain;

public record SessionData
{
    public Guid SessionId { get; set; }
    public string SessionName { get; set; } 
    public List<Question?> Questions { get; set; }
    public int CurrentQuestionIndex { get; set; }
    public SessionState CurrentState { get; set; }
    
    public HashSet<UserAnswer> UserAnswers { get; set; }

    public Dictionary<int, int> GetAnswersCount() =>
        UserAnswers
            .Where(a => a.QuestionId == Questions[CurrentQuestionIndex]!.Id)
            .GroupBy(a => a.AnswerId)
            .ToDictionary(g => g.Key, g => g.Count());
}