namespace KotB.StatePattern
{
    public class EventPredicate : IPredicate
    {
        private bool triggered = false;

        public bool Evaluate() {
            if (triggered) {
                triggered = false;
                return true;
            }
            return false;
        }

        public void Trigger() {
            triggered = true;
        }
    }

    public class EventPredicate<T> : IPredicate
    {
        private bool triggered = false;
        private T lastValue;

        public bool Evaluate() {
            if (triggered) {
                triggered = false;
                return true;
            }
            return false;
        }

        public void Trigger(T value) {
            lastValue = value;
            triggered = true;
        }

        public T LastValue => lastValue;
    }
}
