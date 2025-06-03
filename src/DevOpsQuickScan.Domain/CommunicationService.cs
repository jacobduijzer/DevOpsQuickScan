namespace DevOpsQuickScan.Domain;

public class CommunicationService
{
    public event Action<Participant>? OnParticipantJoined;
    public event Action<QuestionWithAnswers>? OnQuestionAsked;
    
    public event Action<QuestionAnswer>? OnQuestionAnswered;

    private readonly ICommunicationEvents _communicationEvents;
    
    public CommunicationService(ICommunicationEvents communicationEvents)
    {
        _communicationEvents = communicationEvents;
        _communicationEvents.OnParticipantJoined += participant =>
            OnParticipantJoined?.Invoke(participant);
        _communicationEvents.OnQuestionAsked += question =>
            OnQuestionAsked?.Invoke(question);
        _communicationEvents.OnQuestionAnswered += answer =>
            OnQuestionAnswered?.Invoke(answer);
    }
    
    public async Task Start(Uri hubUri) =>
        await _communicationEvents.Start(hubUri);
    
    public async Task Join(Guid sessionId, string displayName) =>
        await _communicationEvents.Join(sessionId, displayName);
    
    public async Task AskQuestion(Guid sessionId, QuestionWithAnswers questionWithAnswers) =>
        await _communicationEvents.AskQuestion(sessionId, questionWithAnswers);
}