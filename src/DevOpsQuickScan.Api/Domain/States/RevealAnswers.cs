namespace DevOpsQuickScan.Api.Domain.States;

public class RevealAnswers : State
{
    public RevealAnswers(SessionContext context) : base(context)
    {
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}