using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class DefenseState : AIBaseState
    {
        public DefenseState(AI ai) : base(ai) { }
        
        public override void Enter() {
            switch (ai.Skills.PlayerPosition) {
                case PositionType.Blocker:
                    ai.StateMachine.ChangeState(ai.DefenseBlockerState);
                    break;
                case PositionType.Defender:
                    ai.StateMachine.ChangeState(ai.DefenseDefenderState);
                    break;
                default:
                    Debug.Log($"AI {ai.Skills.AthleteName} has position type {ai.Skills.PlayerPosition} which is not accounted for in DefenseState.");
                    break;
            }
        }
    }
}
