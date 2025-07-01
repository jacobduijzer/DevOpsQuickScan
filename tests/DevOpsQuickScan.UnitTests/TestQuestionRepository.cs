using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.UnitTests;

public static class TestQuestionRepository
{
    public static List<Question> Questions => [
        new Question(1, "What is your favorite color?", "http://www.test.com", []), 
        new Question(2, "What is your favorite food?", "http://www.test.com", [])
    ];
}