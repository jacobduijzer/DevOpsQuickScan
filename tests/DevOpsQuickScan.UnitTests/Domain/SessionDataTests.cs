using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.UnitTests.Domain;

public class SessionDataTests
{
    [Theory]
    [MemberData(nameof(GetData), parameters: 1)]
    public void CanCountVotes(HashSet<UserAnswer> answers)
    {
        // Arrange
        var sessionData = new SessionData
        {
            SessionCode = CodeGenerator.GenerateCode(),
            UserAnswers = new(),
            Questions = TestQuestionRepository.Questions!,
            CurrentQuestionIndex = 0
        };

        sessionData.UserAnswers = answers;

        // Act
        var result = sessionData.GetAnswersCount();

        // Assert
        Assert.Equal(2, result.GetValueOrDefault(1, 0));
        Assert.Equal(1, result.GetValueOrDefault(2, 0));
        Assert.Equal(1, result.GetValueOrDefault(5, 0));
    }

    public static IEnumerable<object[]> GetData(int numTests)
    {
        var sessionCode = string.Empty;
        var allData = new List<object[]>
        {
            new object[]
            {
                new HashSet<UserAnswer>
                {
                    new UserAnswer
                    {
                        SessionCode = sessionCode, 
                        UserId = Guid.NewGuid(),
                        QuestionId = 1,
                        AnswerId = 1
                    },
                    new UserAnswer
                    {
                        SessionCode = sessionCode,
                        UserId = Guid.NewGuid(),
                        QuestionId = 1,
                        AnswerId = 1
                    },
                    new UserAnswer
                    {
                        SessionCode = sessionCode,
                        UserId = Guid.NewGuid(),
                        QuestionId = 1,
                        AnswerId = 2
                    },
                    new UserAnswer
                    {
                        SessionCode = sessionCode,
                        UserId = Guid.NewGuid(),
                        QuestionId = 1,
                        AnswerId = 5
                    },
                }
            },
        };

        return allData.Take(numTests);
    }
}