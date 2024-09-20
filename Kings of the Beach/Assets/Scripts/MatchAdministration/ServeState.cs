using UnityEngine;
using RoboRyanTron.Unite2017.Events;

namespace KotB.StateMachine
{
    public class ServeState : State
    {
        private InputReader inputReader;
        private GameEvent showPowerMeter;

        public ServeState(InputReader inputReader, GameEvent showPowerMeter) {
            this.inputReader = inputReader;
            this.showPowerMeter = showPowerMeter;
        }

        public override void Enter() {
            Debug.Log("Entering the Serve state.");

            inputReader.EnableGameplayInput();
            showPowerMeter.Raise();
        }

        public override void Exit() {
            Debug.Log("Exiting the Serve State.");
        }

        public override void Update() {}
    }
}
