using DevOpsQuickScan.Web.Surveys;
using Microsoft.AspNetCore.SignalR;

namespace DevOpsQuickScan.Web.Sessions;

public class SessionHub : Hub
{
    public async Task JoinSession(string sessionId, string displayName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        Participant participant = new(Context.ConnectionId, displayName);
        await Clients.Group(sessionId).SendAsync("ParticipantJoined", participant);
    }

    public async Task SendQuestion(string sessionId, Question question) =>
        await Clients.Group(sessionId).SendAsync("NewQuestion", question);

    public async Task SendAnswer(string sessionId, Guid questionId, Guid answerId)
    {
        ParticipantAnswer participantAnswer = new(sessionId, Context.ConnectionId, questionId, answerId);
        await Clients.Group(sessionId).SendAsync("NewAnswer", participantAnswer);
    }
}