using DevOpsQuickScan.Domain;
using Microsoft.AspNetCore.Components;

namespace DevOpsQuickScan.Web.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private string _sessionName = string.Empty;

    private void StartSession()
    {
        NavigationManager.NavigateTo($"/survey/{_sessionName}");
    }
}