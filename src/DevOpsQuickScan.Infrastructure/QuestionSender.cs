using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Infrastructure;

public class QuestionSender : IQuestionSender
{
    public Task Send(string sessionCode, Question question)
    {
        throw new NotImplementedException();
    }
}