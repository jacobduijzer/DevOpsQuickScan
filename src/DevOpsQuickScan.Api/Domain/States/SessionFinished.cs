namespace DevOpsQuickScan.Api.Domain.States;

public class SessionFinished : State
{
    public SessionFinished(SessionContext context) : base(context)
    {
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}