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
            inputReader.EnableGameplayInput();
        }

        public override void Update() {}
    }
}
