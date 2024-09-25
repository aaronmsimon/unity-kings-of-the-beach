using UnityEngine;

namespace KotB.StatePattern.MatchStates
{
    public class InPlayState : MatchBaseState
    {
        private InputReader inputReader;

        public InPlayState(InputReader inputReader) {
            this.inputReader = inputReader;
        }

        public override void Enter() {
            Debug.Log("Entering the In Play state.");

            inputReader.EnableGameplayInput();
        }

        public override void Exit() {
            Debug.Log("Exiting the In Play State.");
        }

        public override void Update() {}
    }
}
