namespace DevOpsQuickScan.Domain;

public record QuestionAnswer(Guid ParticipantId, int Question, int Answer);