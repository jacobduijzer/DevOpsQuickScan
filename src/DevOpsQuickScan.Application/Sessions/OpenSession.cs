using DevOpsQuickScan.Domain.Sessions;

namespace DevOpsQuickScan.Application.Sessions;

public class OpenSession(ISessionReader sessionReader)
{
    public record Command(Guid SessionId);

    public async Task Handler(Command command)
    {
        throw new NotImplementedException();
    }
}