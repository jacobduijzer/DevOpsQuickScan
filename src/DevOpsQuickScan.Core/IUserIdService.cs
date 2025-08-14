namespace DevOpsQuickScan.Core;

public interface IUserIdService
{
    Task<string> GetAsync();
}