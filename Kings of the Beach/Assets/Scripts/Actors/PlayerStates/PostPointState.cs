using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.PlayerStates
{
    public class PostPointState : PlayerBaseState
    {
        public PostPointState(Player player) : base(player) { }

        public override void Enter() {
            player.MatchInfo.TransitionToPrePointState += OnTransitionToPrePointState;
        }

        public override void Exit() {
            player.MatchInfo.TransitionToPrePointState -= OnTransitionToPrePointState;
        }

        private void OnTransitionToPrePointState() {
            player.StateMachine.ChangeState(player.NormalState);
        }
    }
}
