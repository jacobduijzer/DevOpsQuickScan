namespace DevOpsQuickScan.Domain;

public enum SessionState
{
    Initial,
    Started,
    AwaitAnswers,
    AnswersRevealed,
    Completed
}