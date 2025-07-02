namespace DevOpsQuickScan.Domain;

public interface IAnswersSender
{
    Task Send(Guid sessionId, Question question, Dictionary<int, int> answers);
}