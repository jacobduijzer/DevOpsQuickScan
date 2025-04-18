using DevOpsQuickScan.Web.Surveys;
using Microsoft.AspNetCore.SignalR.Client;

namespace DevOpsQuickScan.Web.Sessions;

public class HubConnectionWrapper : IHubConnectionWrapper, IAsyncDisposable
{
    public event Action<Participant>? OnParticipantJoined;
    public event Action<Question>? OnNewQuestion;
    public event Action<ParticipantAnswer>? OnNewAnswer;

    private HubConnection? _hubConnection;
    
    public async Task Start(string sessionId, string hubUrl)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<Participant>("ParticipantJoined", participant =>
            OnParticipantJoined?.Invoke(participant));

        _hubConnection.On<Question>("NewQuestion", question =>
            OnNewQuestion?.Invoke(question));

        _hubConnection.On<ParticipantAnswer>("NewAnswer", answer =>
            OnNewAnswer?.Invoke(answer));
        
        await _hubConnection.StartAsync();
    }

    public async Task JoinSession(string sessionId, string displayName) => 
        await _hubConnection?.SendAsync("JoinSession", sessionId, displayName)!;

    public async Task SendQuestion(string sessionId, Question question) =>
       await _hubConnection?.SendAsync("SendQuestion", sessionId, question)!; 
    
    public async Task SendAnswer(string sessionId, Guid questionId, Guid answerId) =>
        await _hubConnection?.SendAsync("SendAnswer", sessionId, questionId, answerId)!;

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null) 
            await _hubConnection.DisposeAsync();
    }
}