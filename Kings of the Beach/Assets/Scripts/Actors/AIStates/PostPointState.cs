using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class PostPointState : AIBaseState
    {
        public PostPointState(AI ai) : base(ai) { }

        public override void Enter() {
            ai.MoveDir = Vector3.zero;
            
            ai.MatchInfo.TransitionToServeState += OnMatchChangeToServeState;
        }

        public override void Exit() {
            ai.MatchInfo.TransitionToServeState -= OnMatchChangeToServeState;
        }

        private void OnMatchChangeToServeState() {
            if (ai.MatchInfo.Server == ai) {
                ai.StateMachine.ChangeState(ai.ServeState);
            } else {
                Vector3 newPos = ai.MatchInfo.Server.CourtSide == ai.CourtSide ? ai.Skills.ServingPartnerPos : ai.GetMyDefensivePosition(ai.DefensePos);
                ai.transform.position = new Vector3(newPos.x * ai.CourtSide, newPos.y, newPos.z);
                ai.StateMachine.ChangeState(ai.DefenseState);
            }
        }
    }
}
