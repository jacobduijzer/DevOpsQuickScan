using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Infrastructure;

public class AnswerSender : IAnswersSender
{
    public Task Send(string sessionCode, Question question, Dictionary<int, int> answers)
    {
        throw new NotImplementedException();
    }
}