public class StateMachine<T>
{
    public State<T> CurrentState { get; private set; }
    public T Owner;

    public StateMachine(T owner)
    {
        Owner = owner;
    }
    public void ChangeState(State<T> newState)
    {
        if (CurrentState != null)
            CurrentState.ExitState(Owner);
        CurrentState = newState;
        CurrentState.EnterState(Owner);
    }
    public void Upate()
    {
        if (CurrentState != null)
            CurrentState.UpdateState(Owner);
    }
}
