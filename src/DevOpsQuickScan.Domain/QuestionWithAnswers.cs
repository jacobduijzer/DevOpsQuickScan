namespace DevOpsQuickScan.Domain;

public record QuestionWithAnswers(Question Question, List<QuestionAnswer>? Answers = null);