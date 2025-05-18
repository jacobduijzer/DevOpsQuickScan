using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.UnitTests.Stubs;

public class QuestionRepositoryStub : IQuestionRepository
{
    public Task<QuestionData> Get()
    {
        return Task.FromResult(new QuestionData("Test Session", [
            new Question(1, "What is your favorite color?", "https://google.com?q=favorite%20color", [
                new Answer(1, "Red"),
                new Answer(2, "Blue"),
                new Answer(3, "Green")
            ]),
            new Question(2, "What is your favorite animal?", "https://google.com?q=favorite%20animal", [
                new Answer(1, "Dog"),
                new Answer(2, "Cat"),
                new Answer(3, "Bird")
            ]),
            new Question(3, "What is your favorite food?", "https://google.com?q=favorite%20food", [
                new Answer(1, "Pizza"),
                new Answer(2, "Burger"),
                new Answer(3, "Pasta")
            ])
        ]));
    }
}