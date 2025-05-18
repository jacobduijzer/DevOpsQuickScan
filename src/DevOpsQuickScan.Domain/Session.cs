using System.Text.Json.Serialization;

namespace DevOpsQuickScan.Domain;

public class Session
{
   public Guid Id { get; }
   public Guid FacilitatorId { get; }
   public string Name { get; }
   public SessionState CurrentState { get; private set; } = SessionState.NotStarted;
   public List<Question> Questions { get; } = new ();
   public Question? CurrentQuestion { get; private set; }
   public HashSet<QuestionAnswer> Answers { get; } = new();

   public Session(Guid facilitatorId, string name, List<Question> questions)
   {
      Id = Guid.NewGuid();
      FacilitatorId = facilitatorId;
      Name = name;
      Questions = questions;
   }

   [JsonConstructor]
   public Session(
      Guid id, 
      Guid facilitatorId, 
      string name, 
      SessionState currentState,
      List<Question> questions,
      Question currentQuestion,
      HashSet<QuestionAnswer> answers)
   {
      Id = id;
      FacilitatorId = facilitatorId;
      Name = name;
      CurrentState = currentState;
      Questions = questions;
      CurrentQuestion = currentQuestion;
      Answers = answers;
   }

   public void Start()
   {  
      if(CurrentState != SessionState.NotStarted)
         throw new InvalidOperationException("Session already started");
      
      CurrentState = SessionState.QuestionPending;
   }

   public void SelectQuestion(Question question)
   {
      if(CurrentState != SessionState.QuestionPending && CurrentState != SessionState.RevealingAnswers)
         throw new InvalidOperationException("Invalid state for selecting questions");
      
      CurrentState = SessionState.CollectingAnswers;
      CurrentQuestion = question;
   }

   public void AnswerQuestion(Guid participantId, int questionId, int answerId)
   {
      if(CurrentState != SessionState.CollectingAnswers)
         throw new InvalidOperationException("Invalid state for answering questions");
      
      if(CurrentQuestion!.Id != questionId)
         throw new InvalidOperationException("This is not the current question");
      
      if(CurrentQuestion.Answers.All(x => x.Id != answerId))
         throw new InvalidOperationException($"The current question does not contain an answer with id '{answerId}'");

      var questionAnswer = new QuestionAnswer(participantId, questionId, answerId);
      Answers.Add(questionAnswer);
   }

   public void RevealingAnswers()
   {
      if(CurrentState != SessionState.CollectingAnswers)
         throw new InvalidOperationException("Invalid state for revealing answers");
      
      CurrentState = SessionState.RevealingAnswers;
   }
   
   public void End() =>
      CurrentState = SessionState.SessionEnded;
}