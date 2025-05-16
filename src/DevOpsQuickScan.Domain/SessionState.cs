namespace DevOpsQuickScan.Domain;

public enum SessionState
{
    NotStarted,
    QuestionPending,
    CollectingAnswers,
    RevealingAnswers,
    SessionEnded 
}