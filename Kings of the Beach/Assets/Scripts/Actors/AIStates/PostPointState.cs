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
            ai.BallInfo.BallServed += OnBallServed;
        }

        public override void Exit() {
            ai.MatchInfo.TransitionToServeState -= OnMatchChangeToServeState;
            ai.BallInfo.BallServed -= OnBallServed;
        }

        private void OnMatchChangeToServeState() {
            if (ai.MatchInfo.Server == ai) {
                ai.StateMachine.ChangeState(ai.ServeState);
            } else {
                Vector3 newPos = ai.MatchInfo.Server.CourtSide == ai.CourtSide ? ai.Skills.ServingPartnerPos : ai.Skills.ReceivingPos;
                ai.transform.position = new Vector3(newPos.x * ai.CourtSide, newPos.y, newPos.z);
            }
        }

        private void OnBallServed() {
            if (ai.BallInfo.Possession == ai.CourtSide) {
                ai.StateMachine.ChangeState(ai.DefenseState);
            } else {
                ai.StateMachine.ChangeState(ai.DigReadyState);
            }
        }
    }
}
