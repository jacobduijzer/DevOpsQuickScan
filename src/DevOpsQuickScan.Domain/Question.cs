namespace DevOpsQuickScan.Domain;

public record Question(int Id, string Text, string Link, List<Answer> Answers);