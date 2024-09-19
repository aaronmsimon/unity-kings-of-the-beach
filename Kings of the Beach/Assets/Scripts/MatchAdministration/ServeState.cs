using UnityEngine;

namespace KotB.StateMachine
{
    public class ServeState : MatchState
    {
        public ServeState(MatchManager matchManager) : base(matchManager) {
        }

        public override void Enter() {
            Debug.Log("Entering the Serve state.");
            matchManager.InputReader.EnableGameplayInput();
        }

        public override void Exit() {
            Debug.Log("Exiting the Serve State.");
        }

        public override void Update() {
        }
    }
}
