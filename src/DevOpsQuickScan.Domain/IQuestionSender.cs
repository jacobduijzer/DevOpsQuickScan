namespace DevOpsQuickScan.Domain;

public interface IQuestionSender
{
    Task Send(string sessionCode, Question question);
}