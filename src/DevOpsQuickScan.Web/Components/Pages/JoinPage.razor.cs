using DevOpsQuickScan.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace DevOpsQuickScan.Web.Components.Pages;

public partial class JoinPage : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ISessionStore SessionStore { get; set; } = default!;

    private string _sessionName = string.Empty;
    [Parameter]
    [SupplyParameterFromQuery]
    public string? session { get; set; }

    private string? sessionName;
    private string sessionId = "";
    private string userName;
    private bool joined = false;
    private HubConnection? hubConnection;

    protected override async Task OnInitializedAsync()
    {
        if (!string.IsNullOrWhiteSpace(session))
        {
            sessionId = session;
            sessionName = SessionStore.GetSessionName(session);

            if (string.IsNullOrEmpty(sessionName))
            {
                sessionName = null; // Invalid session
            }
        }
    }

    
    private async Task JoinSession()
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(sessionId))
        {
            return; // Ensure both userName and sessionId are valid
        }

        hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/hub/voting"))
            .WithAutomaticReconnect()
            .Build();

        await hubConnection.StartAsync();

        await hubConnection.InvokeAsync("JoinSession", sessionId, userName);

        joined = true;

        // Navigate to the vote page
        NavigationManager.NavigateTo($"/vote/{sessionId}");
    }
}