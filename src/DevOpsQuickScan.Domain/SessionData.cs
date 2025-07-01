namespace DevOpsQuickScan.Domain;

public record SessionData
{
    public Guid SessionId { get; set; }
    public string SessionName { get; set; } 
    public List<Question?> Questions { get; set; }
    public int CurrentQuestionIndex { get; set; }
    public SessionState CurrentState { get; set; }
}