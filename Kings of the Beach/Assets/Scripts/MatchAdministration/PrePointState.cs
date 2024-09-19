using UnityEngine;

namespace KotB.StateMachine
{
    public class PrePointState : MatchState
    {
        public PrePointState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter() {
            Debug.Log("Entering the Pre Point state.");
            matchManager.InputReader.EnableBetweenPointsInput();
            matchManager.InputReader.interactEvent += OnInteract;
        }

        public override void Exit() {
            matchManager.InputReader.interactEvent -= OnInteract;
            Debug.Log("Exiting the Pre Point State.");
        }

        public override void Update() { }

        private void OnInteract() {
            matchManager.MatchStateMachine.ChangeState(matchManager.ServeState);
        }
    }
}
