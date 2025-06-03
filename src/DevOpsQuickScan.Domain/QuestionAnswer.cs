namespace DevOpsQuickScan.Domain;

public record QuestionAnswer(Guid SessionId, int QuestionId, int AnswerId);