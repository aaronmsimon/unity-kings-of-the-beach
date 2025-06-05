using UnityEngine;
using KotB.Match;

namespace KotB.StatePattern.MatchStates
{
    public class PrePointState : MatchBaseState
    {
        public PrePointState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter() {
            matchManager.MatchInfo.TransitionToPrePointStateEvent();

            matchManager.InputReader.EnableBetweenPointsInput();
            matchManager.InputReader.interactEvent += OnInteract;

            matchManager.BallInfo.ClearTargetPos();
        }

        public override void Exit() {
            matchManager.InputReader.interactEvent -= OnInteract;
        }

        private void OnInteract() {
            matchManager.MatchInfo.TransitionToServeStateEvent();
        }
    }
}
