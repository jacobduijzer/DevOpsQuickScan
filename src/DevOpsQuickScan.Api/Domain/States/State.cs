namespace DevOpsQuickScan.Api.Domain.States;

public abstract class State
{
    protected SessionContext Context;

    protected State(SessionContext context)
    {
        Context = context;
    }

    public void SetContext(SessionContext context)
    {
        Context = context;
    }

    public abstract void Execute();
}