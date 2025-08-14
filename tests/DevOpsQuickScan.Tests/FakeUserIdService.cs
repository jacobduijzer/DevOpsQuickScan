using DevOpsQuickScan.Core;

namespace DevOpsQuickScan.Tests;

public class FakeUserIdService : IUserIdService
{
    public Task<string> GetAsync() => 
        Task.FromResult(Guid.NewGuid().ToString());
}