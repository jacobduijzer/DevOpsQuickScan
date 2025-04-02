using DevOpsQuickScan.Domain;
using Microsoft.AspNetCore.SignalR;

namespace DevOpsQuickScan.Infrastructure;

public class VotingHub : Hub
{
    private static readonly Dictionary<string, string> SessionQuestions = new();
    private static readonly Dictionary<string, HashSet<string>> SessionParticipants = new();

    public async Task StartSession(string sessionId)
    {
        SessionParticipants.TryAdd(sessionId, new HashSet<string>());
    }

    public async Task JoinSession(string sessionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);

        var userId = Context.ConnectionId; // or something more friendly later

        if (!SessionParticipants.TryGetValue(sessionId, out var participants))
        {
            participants = new HashSet<string>();
            SessionParticipants[sessionId] = participants;
        }

        participants.Add(userId);
        await Clients.Group(sessionId).SendAsync("ParticipantJoined", userId);

        // Send the current question if available
        if (SessionQuestions.TryGetValue(sessionId, out var currentQuestion))
        {
            await Clients.Caller.SendAsync("ReceiveQuestion", currentQuestion);
        }
    }
    
    public async Task SendQuestion(Question question)
    {
        await Clients.All.SendAsync("ReceiveQuestion", question);
    }

    public async Task SubmitVote(string sessionId, Vote vote)
    {
        await Clients.Group(sessionId).SendAsync("VoteReceived", vote);
    }

    public async Task SetCurrentQuestion(string sessionId, string question)
    {
        SessionQuestions[sessionId] = question;
        await Clients.Group(sessionId).SendAsync("ReceiveQuestion", question);
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"Disconnected: {Context.ConnectionId}");

        foreach (var kvp in SessionParticipants)
        {
            var sessionId = kvp.Key;
            var participants = kvp.Value;

            if (participants.Remove(Context.ConnectionId))
            {
                await Clients.Group(sessionId).SendAsync("ParticipantLeft", Context.ConnectionId);
                break;
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}