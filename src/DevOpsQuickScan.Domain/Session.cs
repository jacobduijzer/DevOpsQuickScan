using System.Text.Json.Serialization;

namespace DevOpsQuickScan.Domain;

public class Session
{
   public Guid Id { get; }
   public Guid FacilitatorId { get; }
   public string SessionName { get; }
   public SessionState CurrentState { get; private set; } = SessionState.NotStarted;
   public Question CurrentQuestion { get; private set; }

   public Session(Guid facilitatorId, string sessionName)
   {
      Id = Guid.NewGuid();
      FacilitatorId = facilitatorId;
      SessionName = sessionName;
   }

   [JsonConstructor]
   public Session(
      Guid id, 
      Guid facilitatorId, 
      string sessionName, 
      SessionState currentState,
      Question currentQuestion)
   {
      Id = id;
      FacilitatorId = facilitatorId;
      SessionName = sessionName;
      CurrentState = currentState;
      CurrentQuestion = currentQuestion;
   }

   public void Start()
   {  
      if(CurrentState != SessionState.NotStarted)
         throw new InvalidOperationException("Session already started");
      CurrentState = SessionState.QuestionPending;
   }

   public void SelectQuestion(Question question)
   {
      if(CurrentState != SessionState.QuestionPending)
         throw new InvalidOperationException("Invalid state for selecting questions");
      CurrentState = SessionState.CollectingAnswers;
      CurrentQuestion = question;
   }
}