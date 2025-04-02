using Microsoft.AspNetCore.SignalR;

namespace DevOpsQuickScan.Infrastructure;

public class VotingHub : Hub
{
    public async Task JoinSession(string sessionId) =>
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);

    public async Task SubmitVote(string sessionId, string option)
    {
        Console.WriteLine($"Received vote: {option} for session: {sessionId}");

        try
        {
            await Clients.Group(sessionId).SendAsync("VoteReceived", option);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Error in SubmitVote: {ex.Message}");
            throw;
        }
    }
}