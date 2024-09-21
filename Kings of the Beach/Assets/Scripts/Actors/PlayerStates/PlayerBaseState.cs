using KotB.Actors;

namespace KotB.StatePattern.PlayerStates
{
    public abstract class PlayerBaseState : IState
    {
        protected readonly Player player;

        protected PlayerBaseState(Player context) {
            this.player = context;
        }

        public virtual void Enter() {
            // no op
        }

        public virtual void Exit() {
            // no op
        }

        public virtual void Update() {
            // no op
        }
    }
}
