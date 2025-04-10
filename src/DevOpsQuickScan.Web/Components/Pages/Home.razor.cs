using DevOpsQuickScan.Application;
using Microsoft.AspNetCore.Components;

namespace DevOpsQuickScan.Web.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ISessionStore SessionStore { get; set; } = default!;

    private string _sessionName = string.Empty;

    private void StartSession()
    {
        var sessionId = SessionStore.CreateSession(_sessionName);
        NavigationManager.NavigateTo($"/survey/{sessionId}");
    }
}