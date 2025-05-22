using DevOpsQuickScan.Domain;
using Microsoft.AspNetCore.SignalR.Client;

namespace DevOpsQuickScan.Infrastructure;

public class CommunicationEvents : ICommunicationEvents
{
    public event Action<Participant>? OnParticipantJoined;

    private HubConnection? _hubConnection;

    public async Task Start(Uri hubUri)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUri)
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<Participant>("ParticipantJoined", participant =>
            OnParticipantJoined?.Invoke(participant));

        await _hubConnection.StartAsync();
    }

    public async Task Join(Guid sessionId, string displayName) =>
        await _hubConnection?.SendAsync("JoinSession", sessionId, displayName)!;

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
            await _hubConnection.DisposeAsync();
    }
}