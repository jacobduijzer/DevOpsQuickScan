
using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.UnitTests.Domain;

public class SessionServiceTests_State_Transitions
{
    [Fact]
    public void CanNotTransitionWithoutSessionName()
    {
        // ARRANGE
        SessionService session = new();
        
        // ACT & ASSERT
        var exception = Assert.Throws<InvalidOperationException>(() => session.Start("", []));
        Assert.Contains("A session name must be set before starting and at least one question must be added.", exception.Message); 
    }
    
    [Fact]
    public void CanNotTransitionWithoutQuestions()
    {
        // ARRANGE
        SessionService session = new();
        
        // ACT & ASSERT
        var exception = Assert.Throws<InvalidOperationException>(() => session.Start("", [new Question(1, "What is your favorite color?")]));
        Assert.Contains("A session name must be set before starting and at least one question must be added.", exception.Message); 
    }
    
    [Fact]
    public void ASessionCanStartWhenNameAndQuestionsAreSet()
    {
        // ARRANGE
        SessionService session = new();
        
        // ACT
        session.Start("Test Session", [new Question(1, "What is your favorite color?"), new Question(2, "What is your favorite food?")]);
        
        // ASSERT
        Assert.Equal(SessionState.Started, session.CurrentState);
    }
    
    // TODO: Dubious test. It is not testing the state machine, but rather the logic of the method.
    [Fact]
    public void CanNotAskQuestionWithoutSettingCurrentQuestion()
    {
        // ARRANGE
        SessionService session = new();
        session.Start("Test Session", [new Question(1, "What is your favorite color?"), new Question(2, "What is your favorite food?")]);
        
        // ACT & ASSERT
        var exception = Assert.Throws<InvalidOperationException>(() => session.AskQuestion());
        Assert.Contains("A question must be selected before asking.", exception.Message); 
    }
    
    [Fact]
    public void CanRevealAnswersAfterAskingQuestion()
    {
        // ARRANGE
        SessionService session = new();
        session.Start("Test Session", [new Question(1, "What is your favorite color?"), new Question(2, "What is your favorite food?")]);
        session.NextQuestion();
        session.AskQuestion();
        
        // ACT
        session.RevealAnswers();
        
        // ASSERT
        Assert.Equal(SessionState.AnswersRevealed, session.CurrentState);
    }
    
    [Fact]
    public void CanAnswerNewQuestionAfterRevealingAnswers()
    {
        // ARRANGE
        SessionService session = new();
        session.Start("Test Session", [new Question(1, "What is your favorite color?"), new Question(2, "What is your favorite food?")]);
        session.NextQuestion();
        session.AskQuestion();
        session.RevealAnswers();
        
        // ACT
        session.AskQuestion();
        
        // ASSERT
        Assert.Equal(SessionState.AwaitAnswers, session.CurrentState);
    }

    [Fact]
    public void CanFinishSessionWithoutAskingQuestions()
    {
        // ARRANGE
        SessionService session = new();
        session.Start("Test Session", [new Question(1, "What is your favorite color?"), new Question(2, "What is your favorite food?")]);
        
        // ACT
        session.Finish();
        
        // ASSERT
        Assert.Equal(SessionState.Completed, session.CurrentState); 
    }
    
    [Fact]
    public void CanFinishSessionWhenAwaitingAnswers()
    {
        // ARRANGE
        SessionService session = new();
        session.Start("Test Session", [new Question(1, "What is your favorite color?"), new Question(2, "What is your favorite food?")]);
        session.NextQuestion();
        session.AskQuestion();
        
        // ACT
        session.Finish();
        
        // ASSERT
        Assert.Equal(SessionState.Completed, session.CurrentState); 
    }
    
    [Fact]
    public void CanFinishSessionWhenRevealingAnswers()
    {
        // ARRANGE
        SessionService session = new();
        session.Start("Test Session", [new Question(1, "What is your favorite color?"), new Question(2, "What is your favorite food?")]);
        session.NextQuestion();
        session.AskQuestion();
        session.RevealAnswers();
        
        // ACT
        session.Finish();
        
        // ASSERT
        Assert.Equal(SessionState.Completed, session.CurrentState); 
    }
}