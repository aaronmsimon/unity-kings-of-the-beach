using UnityEngine;

namespace KotB.StatePattern.MatchStates
{
    public class ServeState : MatchBaseState
    {
        private InputReader inputReader;

        public ServeState(InputReader inputReader) {
            this.inputReader = inputReader;
        }

        public override void Enter() {
            Debug.Log("Entering the Serve state.");

            inputReader.EnableGameplayInput();
        }

        public override void Exit() {
            Debug.Log("Exiting the Serve State.");
        }

        public override void Update() {}
    }
}
