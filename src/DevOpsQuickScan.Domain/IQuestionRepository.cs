namespace DevOpsQuickScan.Domain;

public interface IQuestionRepository
{
    Task<QuestionData> Get();
}