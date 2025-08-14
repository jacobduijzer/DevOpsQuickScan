using DevOpsQuickScan.Core;
using Microsoft.JSInterop;

namespace DevOpsQuickScan.BlazorApp;

public class UserIdService(IJSRuntime js) : IUserIdService
{
    private string? _userId;

    public async Task<string> GetAsync()
    {
        if (_userId is not null)
            return _userId;

        _userId = await js.InvokeAsync<string>("userStorage.getOrCreateUserId");
        return _userId;
    }
}