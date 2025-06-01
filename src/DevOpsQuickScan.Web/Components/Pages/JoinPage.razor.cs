using DevOpsQuickScan.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DevOpsQuickScan.Web.Components.Pages;

public partial class JoinPage : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private ICommunicationEvents CommunicationEvents { get; set; } = default!;
    [Inject] private SessionService SessionService { get; set; } = default!;
    [Parameter] [SupplyParameterFromQuery] public Guid? SessionId { get; set; }

    private string _sessionName = string.Empty;
    private string _displayName = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        _sessionName = await SessionService.SessionName(SessionId.Value);
    }

    private async Task JoinSession()
    {
        if (string.IsNullOrWhiteSpace(_displayName) || !SessionId.HasValue)
            return; // Ensure both userName and sessionId are valid

        

        // Navigate to the vote page
        NavigationManager.NavigateTo($"/vote/{SessionId.Value.ToString()}/{_displayName}");
    }
}