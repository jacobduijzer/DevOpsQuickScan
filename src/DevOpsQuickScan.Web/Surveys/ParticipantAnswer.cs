namespace DevOpsQuickScan.Web.Surveys;

public record ParticipantAnswer(string SessionId, string UserId, Guid QuestionId, Guid AnswerId);