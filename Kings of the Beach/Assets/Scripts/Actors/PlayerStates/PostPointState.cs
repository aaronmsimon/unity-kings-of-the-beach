using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.PlayerStates
{
    public class PostPointState : PlayerBaseState
    {
        public PostPointState(Player player) : base(player) { }

        public override void Enter() {
            // this is obviously temporary for testing with coach
            player.StateMachine.ChangeState(player.NormalState);
        }

        public override void Exit() {
        }
    }
}
