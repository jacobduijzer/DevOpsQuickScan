using DevOpsQuickScan.Domain.Questions;

namespace DevOpsQuickScan.Core;

public class QuestionWithAnswers(Question question)
{
    public readonly Question Question = question;
    public List<RevealedAnswer> Answers { get; set; } = [];
}