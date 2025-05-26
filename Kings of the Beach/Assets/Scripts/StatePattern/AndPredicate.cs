namespace KotB.StatePattern
{
    public class AndPredicate : IPredicate
    {
        private readonly IPredicate[] predicates;

        public AndPredicate(params IPredicate[] predicates) {
            this.predicates = predicates;
        }

        public bool Evaluate() {
            foreach (var predicate in predicates) {
                if (!predicate.Evaluate()) {
                    return false;
                }
            }
            return true;
        }
    }
}
