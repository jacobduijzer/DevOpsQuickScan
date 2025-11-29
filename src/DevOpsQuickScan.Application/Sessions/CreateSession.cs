using DevOpsQuickScan.Domain.Sessions;

namespace DevOpsQuickScan.Application.Sessions;

public class CreateSession(SessionService sessionService)
{
    public record Command(string SessionName, string Email);

    public async Task Handler(Command command)
    {
        throw new NotImplementedException();
    }
}