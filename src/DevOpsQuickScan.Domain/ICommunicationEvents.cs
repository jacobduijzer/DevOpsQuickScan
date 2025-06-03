namespace DevOpsQuickScan.Domain;

public interface ICommunicationEvents : IAsyncDisposable
{
    event Action<Participant>? OnParticipantJoined;
    event Action<QuestionWithAnswers>? OnQuestionAsked;
    event Action<QuestionAnswer>? OnQuestionAnswered;

    
    // event Action<ParticipantA

    Task Start(Uri hubUri);

    Task Join(Guid sessionId, string displayName);

    Task AskQuestion(Guid sessionId, QuestionWithAnswers questionWithAnswers);

    Task AnswerQuestion(QuestionAnswer answer);
    // event Action<ParticipantAnswer>? OnNewAnswer;
    // event Action<int>? OnRevealAnswers;
    //
    // Task Start(string sessionId, string hubUrl);
    // Task JoinSession(string sessionId, string displayName);
    // Task SendAnswer(string sessionId, Guid questionId, Guid answerId);
    // Task RevealAnswers(string sessionId, Question question); 
}