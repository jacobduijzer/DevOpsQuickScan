namespace DevOpsQuickScan.Core;

public class QuestionsService
{
    public void RevealQuestion(int questionId)
    {
        var question = All.FirstOrDefault(q => q.Id == questionId);
        if (question is not null)
            question.IsRevealed = true;
    }
    
    public List<Question> All =>
    [
        new Question(1, "What is your primary programming language?", [
            new Answer(1, "C#"),
            new Answer(2, "Java"),
            new Answer(3, "JavaScript"),
            new Answer(4, "Python"),
            new Answer(5, "Other")
        ]),
        new Question(2, "What is your preferred development environment?", [
            new Answer(1, "Visual Studio"),
            new Answer(2, "Eclipse"),
            new Answer(3, "IntelliJ IDEA"),
            new Answer(4, "VS Code"),
            new Answer(5, "Other")
        ]),
        new Question(3, "What is your favorite food?", [
            new Answer(1, "Pizza"),
            new Answer(2, "Sushi"),
            new Answer(3, "Burgers"),
            new Answer(4, "Salad"),
            new Answer(5, "Other")
        ]),
    ];
}