using DevOpsQuickScan.Domain;

namespace DevOpsQuickScan.Web.Sessions;

public interface IHubConnectionWrapper
{
    event Action<Participant>? OnParticipantJoined;
    event Action<Question>? OnNewQuestion;
    event Action<ParticipantAnswer>? OnNewAnswer;
    
    Task Start(string sessionId, string hubUrl);
    Task JoinSession(string sessionId, string displayName);
    Task SendQuestion(string sessionId, Question question);
    Task SendAnswer(string sessionId, Guid questionId, Guid answerId);
}