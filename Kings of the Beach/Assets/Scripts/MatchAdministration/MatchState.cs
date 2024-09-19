namespace KotB.StateMachine
{
    public abstract class MatchState : State
    {
        protected MatchManager matchManager;

        public MatchState(MatchManager matchManager) {
            this.matchManager = matchManager;
        }
    }
}
