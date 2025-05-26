namespace KotB.StatePattern
{
    public interface ITransition
    {
        public IState To { get; }

        public IPredicate Condition { get; }
    }
}
