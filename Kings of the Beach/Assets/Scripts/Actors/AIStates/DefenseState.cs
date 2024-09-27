using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class DefenseState : AIBaseState
    {
        public DefenseState(AI ai) : base(ai) { }

        public override void Update() {
            if (ai.transform.position != ai.Skills.ReceivingPos) {
                ai.MoveDir = ai.Skills.ReceivingPos - ai.transform.position;
            } else {
                ai.MoveDir = Vector3.zero;
                ai.StateMachine.ChangeState(ai.DigReadyState);
            }
        }
    }
}
