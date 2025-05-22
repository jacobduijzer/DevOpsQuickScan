using DevOpsQuickScan.Domain;
using Microsoft.AspNetCore.SignalR;

namespace DevOpsQuickScan.Infrastructure;

public class SessionHub : Hub
{
    public async Task JoinSession(Guid sessionId, string displayName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId.ToString());
        Participant participant = new(Context.ConnectionId, sessionId, displayName);
        await Clients.Group(sessionId.ToString()).SendAsync("ParticipantJoined", participant);
    }

    // public async Task ParticipantJoined(Participant participant)
    // {
    //     
    // }

    // public async Task SendQuestion(string sessionId, Question question) =>
    //     await Clients.Group(sessionId).SendAsync("NewQuestion", question);
    //
    // public async Task SendAnswer(string sessionId, Guid questionId, Guid answerId)
    // {
    //     ParticipantAnswer participantAnswer = new(sessionId, Context.ConnectionId, questionId, answerId);
    //     await Clients.Group(sessionId).SendAsync("NewAnswer", participantAnswer);
    // }
}