namespace KotB.StatePattern
{
    public class EventPredicate : IPredicate
    {
        private StateMachine stateMachine;
        private bool triggered = false;

        public EventPredicate(StateMachine stateMachine) {
            this.stateMachine = stateMachine;
            this.stateMachine.StateChanged += Reset;
        }

        public bool Evaluate() {
            if (triggered) {
                return true;
            }
            return false;
        }

        public void Trigger() {
            triggered = true;
        }

        public void Cleanup() {
            stateMachine.StateChanged -= Reset;
        }

        private void Reset(IState state) {
            triggered = false;
        }
    }

    public class EventPredicate<T> : IPredicate
    {
        private StateMachine stateMachine;
        private bool triggered = false;
        private T lastValue;

        public EventPredicate(StateMachine stateMachine) {
            this.stateMachine = stateMachine;
            this.stateMachine.StateChanged += Reset;
        }

        public bool Evaluate() {
            if (triggered) {
                return true;
            }
            return false;
        }

        public void Trigger(T value) {
            lastValue = value;
            triggered = true;
        }

        public void Cleanup() {
            stateMachine.StateChanged -= Reset;
        }

        private void Reset(IState state) {
            triggered = false;
        }

        public T LastValue => lastValue;
    }
}
