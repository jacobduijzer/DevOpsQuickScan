namespace DevOpsQuickScan.Domain;

public record ParticipantAnswer(string SessionId, string UserId, Guid QuestionId, Guid AnswerId);