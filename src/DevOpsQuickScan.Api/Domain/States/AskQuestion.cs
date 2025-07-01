namespace DevOpsQuickScan.Api.Domain.States;

public class AskQuestion : State
{
    public AskQuestion(SessionContext context) : base(context)
    {
    }

    public override void Execute()
    {
        Context.TransitionTo(new WaitingForAnswers(Context));
    }
}