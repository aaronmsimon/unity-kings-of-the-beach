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
            inputReader.EnableGameplayInput();
        }

        public override void Update() {}
    }
}
