namespace DevOpsQuickScan.Domain;

public record QuestionAnswer(Guid ParticipantId, int QuestionId, int AnswerId);