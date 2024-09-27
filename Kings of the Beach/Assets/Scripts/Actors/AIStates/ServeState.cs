using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class ServeState : AIBaseState
    {
        public ServeState(AI ai) : base(ai) { }

        public override void Enter() {
        }

        public override void Exit() {
        }

        private void OnServeCompleted() {
            ai.StateMachine.ChangeState(ai.DefenseState);
        }
    }
}
