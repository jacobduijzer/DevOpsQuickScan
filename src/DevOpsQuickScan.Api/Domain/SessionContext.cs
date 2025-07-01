using DevOpsQuickScan.Api.Domain.States;

namespace DevOpsQuickScan.Api.Domain;

public class SessionContext
{
    private State _state = null;

    public SessionContext(State state)
    {
        this.TransitionTo(state);
    }

    // The Context allows changing the State object at runtime.
    public void TransitionTo(State state)
    {
        Console.WriteLine($"Context: Transition to {state.GetType().Name}.");
        // this._state = state;
        // this._state.SetContext(this);
    }

    // The Context delegates part of its behavior to the current State
    // object.
    public void Request1()
    {
        // this._state.Handle1();
    }

    public void Request2()
    {
        // this._state.Handle2();
    }
}