using Microsoft.JSInterop;

namespace DevOpsQuickScan.BlazorApp;

public class UserIdService
{
    private readonly IJSRuntime _js;
    public string? UserId { get; private set; }

    public UserIdService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<string> GetUserIdAsync()
    {
        if (UserId is not null)
            return UserId;

        UserId = await _js.InvokeAsync<string>("userStorage.getOrCreateUserId");
        return UserId;
    }
}