using DevOpsQuickScan.Domain.Questions;

namespace DevOpsQuickScan.Domain.Sessions;

public interface IQuestionReader
{
    Task<List<Question>> Read();
}