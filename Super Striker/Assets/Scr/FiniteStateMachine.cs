public class FiniteStateMachine 
{
    IState currentState;
    
    public void ChangeState(IState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        if (currentState != null) currentState.Enter();
    }

    public void Progress()
    {
        currentState.Execute();
    }

    public void SendAction(Accion accion)
    {
        currentState.ReceiveAction(accion);
    }
}
