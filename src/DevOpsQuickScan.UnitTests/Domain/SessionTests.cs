using System.Text.Json;
using DevOpsQuickScan.Domain;
using DevOpsQuickScan.UnitTests.Stubs;

namespace DevOpsQuickScan.UnitTests.Domain;

public class SessionTests
{
   [Fact]
   public void CanCreateANewSession()
   {
      // ACT
      var session = CreateSession();
      
      // ASSERT
      Assert.NotEqual(Guid.Empty, session.Id);
      Assert.Equal(SessionState.NotStarted, session.CurrentState);
   }

   [Fact]
   public void CanStartANewSession()
   {
      // ARRANGE
      Session session = CreateSession();
      
      // ACT
      session.Start();
      
      // ASSERT
      Assert.Equal(SessionState.QuestionPending, session.CurrentState);
   }
   
   [Fact]
   public void StartWhenAlreadyStartedThrowsInvalidOperationException()
   {
      // ARRANGE
      var session = CreateSession();
      session.Start();

      // ACT & ASSERT
      var exception = Assert.Throws<InvalidOperationException>(() => session.Start());
      Assert.Equal("Session already started", exception.Message);
   }
   
   [Fact]
   public void SelectQuestionOnInvalidStateThrowsInvalidOperationException()
   {
      // ARRANGE
      var question = new Question(1, "Question 1", "https://google.com",new List<Answer>());
      var session = CreateSession();
      session.Start();
      session.SelectQuestion(question);

      // ACT & ASSERT
      var exception = Assert.Throws<InvalidOperationException>(() => session.SelectQuestion(question));
      Assert.Equal("Invalid state for selecting questions", exception.Message);
   }
   
   [Fact]
   public void AnswerQuestionWhenNotCollectingAnswersThrowsInvalidOperationException()
   {
      // ARRANGE
      var question = new Question(1, "Question 1", "https://google.com", new List<Answer>());
      var session = CreateSession();
      session.Start();

      // ACT & ASSERT
      var exception = Assert.Throws<InvalidOperationException>(() => session.AnswerQuestion(Guid.NewGuid(), 1, 1));
      Assert.Equal("Invalid state for answering questions", exception.Message);
   }

   [Fact]
   public void AnswerTheWrongQuestionThrowsInvalidOperationException()
   {
      // ARRANGE
      var question = new Question(1, "Question 1", "https://google.com", new List<Answer>());
      var session = CreateSession();
      session.Start();
      session.SelectQuestion(question);

      // ACT & ASSERT
      var exception = Assert.Throws<InvalidOperationException>(() => session.AnswerQuestion(Guid.NewGuid(), 2, 1));
      Assert.Equal("This is not the current question", exception.Message); 
   }
   
   [Fact]
   public void AnswerAnNonExistingAnswerThrowsInvalidOperationException()
   {
      // ARRANGE
      var question = new Question(1, "Question 1", "https://google.com", [new Answer(1, "Answer 1")]);
      var session = CreateSession();
      session.Start();
      session.SelectQuestion(question);

      // ACT & ASSERT
      var exception = Assert.Throws<InvalidOperationException>(() => session.AnswerQuestion(Guid.NewGuid(), 1, 5));
      Assert.Equal($"The current question does not contain an answer with id '5'", exception.Message); 
   }
   
   [Fact]
   public void RevealingAnswersWhenNotInCollectingStateThrowsInvalidOperationException()
   {
      // ARRANGE
      var session = CreateSession();
      session.Start();

      // ACT & ASSERT
      var exception = Assert.Throws<InvalidOperationException>(() => session.RevealingAnswers());
      Assert.Equal("Invalid state for revealing answers", exception.Message);
   }

   [Fact]
   public void FullSessionTest()
   {
      // ARRANGE
      var participant1Id = Guid.NewGuid();
      var participant2Id = Guid.NewGuid();
      var question1 = new Question(1, "Question 1", "https://google.com", [
         new Answer(1, "Answer 1"), 
         new Answer(2, "Answer 2"), 
         new Answer(3, "Answer 3") 
      ]);
      var question2 = new Question(2, "Question 2", "https://google.com", [
         new Answer(1, "Answer 1"),
         new Answer(2, "Answer 2"),
         new Answer(3, "Answer 3")
      ]);
      var session = CreateSession();
      
      // ACT
      session.Start();
      session.SelectQuestion(question1);
      session.AnswerQuestion(participant1Id, question1.Id, question1.Answers.First().Id);
      session.AnswerQuestion(participant2Id, question1.Id, question1.Answers.Last().Id);
      session.RevealingAnswers();
      session.SelectQuestion(question2);
      session.AnswerQuestion(participant1Id, question2.Id, question2.Answers.Last().Id);
      session.AnswerQuestion(participant2Id, question2.Id, question2.Answers.First().Id);
      session.RevealingAnswers();
      session.End();
      
      // ASSERT
      Assert.Equal(SessionState.SessionEnded, session.CurrentState);
   }

   [Fact]
   public void CanConstructSessionFromJson()
   {
      // ARRANGE
      var participant1Id = Guid.NewGuid();
      var participant2Id = Guid.NewGuid();
      var question1 = new Question(1, "Question 1", "https://google.com?q=question%202", [
         new Answer(1, "Answer 1"), 
         new Answer(2, "Answer 2"), 
         new Answer(3, "Answer 3") 
      ]);
      var question2 = new Question(2, "Question 2", "https://google.com?q=question%202", [
         new Answer(1, "Answer 1"),
         new Answer(2, "Answer 2"),
         new Answer(3, "Answer 3")
      ]);
      var session = CreateSession();
      
      // ACT
      session.Start();
      session.SelectQuestion(question1);
      session.AnswerQuestion(participant1Id, question1.Id, question1.Answers.First().Id);
      session.AnswerQuestion(participant2Id, question1.Id, question1.Answers.Last().Id);
      session.RevealingAnswers();
      session.SelectQuestion(question2);
      session.AnswerQuestion(participant1Id, question2.Id, question2.Answers.Last().Id);
      session.AnswerQuestion(participant2Id, question2.Id, question2.Answers.First().Id);
      session.RevealingAnswers();
      session.End();
      
      // ACT
      var json = JsonSerializer.Serialize(session);
      var deserializedSession = JsonSerializer.Deserialize<Session>(json);

      // ASSERT
      Assert.NotNull(deserializedSession);
      Assert.Equal(session.Id, deserializedSession.Id);
      Assert.Equal(session.Name, deserializedSession.Name);
      Assert.Equal(session.CurrentState, deserializedSession.CurrentState);
      Assert.Equal(session.Answers.Count, deserializedSession.Answers.Count);
   }
   
   private Session CreateSession() =>
      new Session(Guid.NewGuid(), "Test Session", new QuestionRepositoryStub().Get().Result.Questions);
   
}