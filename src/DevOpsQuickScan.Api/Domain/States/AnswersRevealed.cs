namespace DevOpsQuickScan.Api.Domain.States;

public class AnswersRevealed : State
{
    public AnswersRevealed(SessionContext context) : base(context)
    {
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}