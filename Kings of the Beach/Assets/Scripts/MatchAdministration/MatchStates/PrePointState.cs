using UnityEngine;
using RoboRyanTron.Unite2017.Events;

namespace KotB.StatePattern.MatchStates
{
    public class PrePointState : MatchBaseState
    {
        private InputReader inputReader;
        private GameEvent changeToServeState;

        public PrePointState(InputReader inputReader, GameEvent changeToServeState) {
            this.inputReader = inputReader;
            this.changeToServeState = changeToServeState;
        }

        public override void Enter() {
            inputReader.EnableBetweenPointsInput();
            inputReader.interactEvent += OnInteract;
        }

        public override void Exit() {
            inputReader.interactEvent -= OnInteract;
        }

        private void OnInteract() {
            changeToServeState.Raise();
        }
    }
}
