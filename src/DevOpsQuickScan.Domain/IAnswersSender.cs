namespace DevOpsQuickScan.Domain;

public interface IAnswersSender
{
    Task Send(string sessionCode, Question question, Dictionary<int, int> answers);
}