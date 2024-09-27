using UnityEngine;
using KotB.Match;

namespace KotB.StatePattern.MatchStates
{
    public class PostPointState : MatchBaseState
    {
        public PostPointState(MatchManager matchManager) : base(matchManager) { }

        private float timeUntilChangeState;
        private float waitTime = 3f;

        public override void Enter() {
            // matchManager.InputReader.EnableBetweenPointsInput();
            // matchManager.InputReader.interactEvent += OnInteract;

            timeUntilChangeState = waitTime;
        }

        public override void Exit() {
            // matchManager.InputReader.interactEvent -= OnInteract;
        }

        public override void Update() {
            timeUntilChangeState -= Time.deltaTime;

            if (timeUntilChangeState < 0) {
                matchManager.StateMachine.ChangeState(matchManager.PrePointState);
                matchManager.MatchInfo.TransitionToPrePointStateEvent();
            }
        }

        private void OnInteract() {
            // matchManager.MatchInfo.TransitionToServeStateEvent();
            // matchManager.StateMachine.ChangeState(matchManager.ServeState);
        }
    }
}
