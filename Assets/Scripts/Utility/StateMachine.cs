namespace Utility
{
    public class StateMachine<T>
    {
        public State<T> currentState;
        private T _owner;

        public StateMachine(T owner)
        {
            _owner = owner;
        }
        
        public void ChangeState(State<T> newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter(_owner);
        }

        public void ExecuteState()
        {
            currentState?.Execute();
        }
    }
}