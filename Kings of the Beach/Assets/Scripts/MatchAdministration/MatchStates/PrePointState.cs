using UnityEngine;
using KotB.Match;

namespace KotB.StatePattern.MatchStates
{
    public class PrePointState : MatchBaseState
    {
        public PrePointState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter() {
            Debug.Log("Pre Point State");
            matchManager.InputReader.EnableBetweenPointsInput();
            matchManager.InputReader.interactEvent += OnInteract;

            matchManager.BallInfo.ClearTargetPos();
        }

        public override void Exit() {
            matchManager.InputReader.interactEvent -= OnInteract;
        }

        private void OnInteract() {
            matchManager.MatchInfo.TransitionToServeStateEvent();
            matchManager.StateMachine.ChangeState(matchManager.ServeState);
        }
    }
}
