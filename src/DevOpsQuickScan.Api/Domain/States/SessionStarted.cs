namespace DevOpsQuickScan.Api.Domain.States;

public class SessionStarted : State
{
    public SessionStarted(SessionContext context) : base(context)
    {
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}