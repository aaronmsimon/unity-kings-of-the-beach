using UnityEngine;
using RoboRyanTron.Unite2017.Events;

namespace KotB.StateMachine
{
    public class PrePointState : State
    {
        private InputReader inputReader;
        private GameEvent changeToServeState;

        public PrePointState(InputReader inputReader, GameEvent changeToServeState) {
            this.inputReader = inputReader;
            this.changeToServeState = changeToServeState;
        }

        public override void Enter() {
            Debug.Log("Entering the Pre Point state.");

            inputReader.EnableBetweenPointsInput();
            inputReader.interactEvent += OnInteract;
        }

        public override void Exit() {
            inputReader.interactEvent -= OnInteract;

            Debug.Log("Exiting the Pre Point State.");
        }

        public override void Update() {}

        private void OnInteract() {
            changeToServeState.Raise();
        }
    }
}
