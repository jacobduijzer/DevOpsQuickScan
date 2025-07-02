namespace DevOpsQuickScan.Domain;

public interface IQuestionsRepository
{
   Task<List<Question>> All();
}