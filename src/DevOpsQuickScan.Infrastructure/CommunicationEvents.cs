using DevOpsQuickScan.Domain;
using Microsoft.AspNetCore.SignalR.Client;

namespace DevOpsQuickScan.Infrastructure;

public class CommunicationEvents : ICommunicationEvents
{
    public event Action<Participant>? OnParticipantJoined;
    public event Action<QuestionWithAnswers>? OnQuestionAsked;
    public event Action<QuestionAnswer>? OnQuestionAnswered;

    private HubConnection? _hubConnection;

    public async Task Start(Uri hubUri)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUri)
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<Participant>("ParticipantJoined", participant =>
            OnParticipantJoined?.Invoke(participant));

        _hubConnection.On<QuestionWithAnswers>("QuestionAsked", questionWithAnswers =>
            OnQuestionAsked?.Invoke(questionWithAnswers));

        _hubConnection.On<QuestionAnswer>("QuestionAnswered", answer =>
            OnQuestionAnswered?.Invoke(answer));

        await _hubConnection.StartAsync();
    }

    public async Task Join(Guid sessionId, string displayName) =>
        await _hubConnection?.SendAsync("JoinSession", sessionId, displayName)!;

    public async Task AskQuestion(Guid sessionId, QuestionWithAnswers questionWithAnswers) =>
        await _hubConnection?.SendAsync("AskQuestion", sessionId, questionWithAnswers)!;

    public async Task AnswerQuestion(QuestionAnswer answer) =>
        await _hubConnection?.SendAsync("AnswerQuestion", answer)!;

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
            await _hubConnection.DisposeAsync();
    }
}