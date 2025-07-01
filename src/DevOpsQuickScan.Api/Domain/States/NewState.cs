namespace DevOpsQuickScan.Api.Domain.States;

public class NewState : State
{
    public NewState(SessionContext context) : base(context)
    {
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}