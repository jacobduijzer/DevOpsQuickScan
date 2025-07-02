using DevOpsQuickScan.Domain;
using Moq;
using Xunit.Abstractions;

namespace DevOpsQuickScan.UnitTests.Domain;

public class SessionServiceTests_Questions(ITestOutputHelper outputHelper)
{
    [Fact]
    public async Task CanNotGoToPreviousQuestionWhenStarting()
    {
        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);

        // ACT
        var question = sessionService.PreviousQuestion();

        // ASSERT
        Assert.Null(question);
    }
    
    [Fact]
    public async Task CanNotGoBeyondLastQuestion()
    {

        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        sessionService.NextQuestion();
        sessionService.NextQuestion();

        // ACT
        var question = sessionService.NextQuestion();

        // ASSERT
        Assert.Null(question);
    }

    [Fact]
    public async Task CanSelectQuestions()
    {

        // ARRANGE
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper));
        await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        
        // ACT
        var question = sessionService.NextQuestion();
        
        // ASSERT
        Assert.NotNull(question);
        Assert.Equal(1, question.Id);
    }
    
    [Fact]
    public async Task AskQuestionWillFireAnEvent()
    {

        // ARRANGE
        var mockQuestionSender = new Mock<IQuestionSender>();
        SessionService sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper), mockQuestionSender.Object);
        var sessionId = await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        sessionService.NextQuestion();
        
        // ACT
        await sessionService.AskQuestion();
        
        // ASSERT
        mockQuestionSender
            .Verify(x => 
                x.Send(
                    It.Is<Guid>(g => g.Equals(sessionId)), 
                    It.Is<Question>(q => q.Id.Equals(TestQuestionRepository.Questions[0].Id))), 
                Times.Once);
    }
    
    [Fact]
    public async Task RevealAnswersWillFireAnEvent()
    {

        // ARRANGE
        var mockQuestionSender = new Mock<IQuestionSender>();
        var mockAnswersSender = new Mock<IAnswersSender>();
        var sessionService = SessionServiceCreator.Create(new XunitLogger<SessionService>(outputHelper), mockQuestionSender.Object, mockAnswersSender.Object);
        var sessionId = await sessionService.Start("Test Session", TestQuestionRepository.Questions!);
        var question = sessionService.NextQuestion();
        await sessionService.AskQuestion();
        
        await sessionService.AddAnswer(new UserAnswer
        { 
            SessionId = sessionId,
            UserId = Guid.NewGuid(),
            QuestionId = question!.Id,
            AnswerId = question.Answers.First().Id
        });
        
        await sessionService.AddAnswer(new UserAnswer
        { 
            SessionId = sessionId,
            UserId = Guid.NewGuid(),
            QuestionId = question!.Id,
            AnswerId = question.Answers.Last().Id
        });
        
        // ACT
        await sessionService.RevealAnswers();
        
        // ASSERT
        Assert.Equal(SessionState.AnswersRevealed, sessionService.CurrentState);
        mockAnswersSender
            .Verify(x => 
                x.Send(
                    It.Is<Guid>(g => g.Equals(sessionId)), 
                    It.Is<Question>(q => q.Id.Equals(question!.Id)), 
                    It.Is<Dictionary<int, int>>(a=> 
                        a.GetValueOrDefault(question!.Answers.First().Id) == 1 && 
                        a.GetValueOrDefault(question!.Answers.Last().Id) == 1)
                    ), 
                Times.Once);
    }

}