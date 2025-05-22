namespace DevOpsQuickScan.Domain;

public class CommunicationService
{
    public event Action<Participant>? OnParticipantJoined;

    private readonly ICommunicationEvents _communicationEvents;
    
    public CommunicationService(ICommunicationEvents communicationEvents)
    {
        _communicationEvents = communicationEvents;
        _communicationEvents.OnParticipantJoined += participant =>
            OnParticipantJoined?.Invoke(participant);
    }
    
    public async Task Start(Uri hubUri) =>
        await _communicationEvents.Start(hubUri);
    
    public async Task Join(Guid sessionId, string displayName) =>
        await _communicationEvents.Join(sessionId, displayName);
}