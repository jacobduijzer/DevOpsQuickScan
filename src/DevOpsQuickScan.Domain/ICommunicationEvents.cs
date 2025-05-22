namespace DevOpsQuickScan.Domain;

public interface ICommunicationEvents : IAsyncDisposable
{
    event Action<Participant>? OnParticipantJoined;

    Task Start(Uri hubUri);

    Task Join(Guid sessionId, string displayName);

    // event Action<Question>? OnNewQuestion;
    // event Action<ParticipantAnswer>? OnNewAnswer;
    // event Action<int>? OnRevealAnswers;
    //
    // Task Start(string sessionId, string hubUrl);
    // Task JoinSession(string sessionId, string displayName);
    // Task SendQuestion(string sessionId, Question question);
    // Task SendAnswer(string sessionId, Guid questionId, Guid answerId);
    // Task RevealAnswers(string sessionId, Question question); 
}