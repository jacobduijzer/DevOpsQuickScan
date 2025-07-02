namespace DevOpsQuickScan.Domain;

public interface IQuestionSender
{
    Task Send(Guid sessionId, Question question);
}