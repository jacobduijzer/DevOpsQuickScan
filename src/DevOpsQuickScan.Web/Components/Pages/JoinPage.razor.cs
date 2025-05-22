using DevOpsQuickScan.Domain;
using Microsoft.AspNetCore.Components;

namespace DevOpsQuickScan.Web.Components.Pages;

public partial class JoinPage : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
   
    [Inject]
    private ICommunicationEvents CommunicationEvents { get; set; } = default!;
    
    [Inject]
    private SessionService SessionService { get; set; } = default!;
    
    
    [Parameter]
    [SupplyParameterFromQuery]
    public Guid? SessionId { get; set; }
    
    private string _sessionName = string.Empty;
    private string _displayName = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        // if(!SessionId.HasValue)
            // do stuff

            _sessionName = await SessionService.SessionName(SessionId.Value);
            

            // var session = SessionStore.GetSessionName(SessionId!.Value);

//         if (!SesisonId.HasValue))
//             return; // SessionId is required
//             
//         {
//             sessionId = session;€£
//             sessionName = SessionStore.GetSessionName(session);
// €
//             if (string.IsNullOrEmpty(sessionName))
//             {
//                 sessionName = null; // Invalid session
//             }
//         }
    }

    
    private async Task JoinSession()
    {
        if (string.IsNullOrWhiteSpace(_displayName) || !SessionId.HasValue)
            return; // Ensure both userName and sessionId are valid
        
        await CommunicationEvents.Start(NavigationManager.ToAbsoluteUri("/hub/voting"));
        await CommunicationEvents.Join(SessionId.Value, _displayName!);

        // Navigate to the vote page
       // NavigationManager.NavigateTo($"/vote/{SessionId.Value.ToString()}/{_displayName}");
    }
}