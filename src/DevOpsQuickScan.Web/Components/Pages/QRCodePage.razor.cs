using Microsoft.AspNetCore.Components;

namespace DevOpsQuickScan.Web.Components.Pages;

public partial class QRCodePage : ComponentBase
{
    
    [Parameter] 
    public string InviteLink { get; set; } = string.Empty;
}