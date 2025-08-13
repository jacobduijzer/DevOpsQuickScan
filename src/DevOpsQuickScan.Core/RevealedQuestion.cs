namespace DevOpsQuickScan.Core;

public class RevealedQuestion(Question question)
{
    public readonly Question Question = question;
    public List<RevealedAnswer> Answers { get; set; } = [];
}