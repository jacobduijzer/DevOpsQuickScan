namespace DevOpsQuickScan.Api.Domain.States;

public class WaitingForAnswers : State
{
    public WaitingForAnswers(SessionContext context) : base(context)
    {
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}