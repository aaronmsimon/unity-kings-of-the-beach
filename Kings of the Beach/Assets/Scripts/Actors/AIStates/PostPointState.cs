using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class PostPointState : AIBaseState
    {
        public PostPointState(AI ai) : base(ai) { }

        public override void Enter() {
            ai.TargetPos = ai.transform.position;
            
            ai.MatchInfo.TransitionToServeState += OnMatchChangeToServeState;
        }

        public override void Exit() {
            ai.MatchInfo.TransitionToServeState -= OnMatchChangeToServeState;
        }

        private void OnMatchChangeToServeState() {
            if (ai.MatchInfo.Teams[ai.MatchInfo.TeamServeIndex] == ai.MatchInfo.GetTeam(ai)) {
                if (ai.MatchInfo.GetServer() == ai) {
                    ai.StateMachine.ChangeState(ai.ServePosState);
                } else {
                    ai.StateMachine.ChangeState(ai.NonServeState);
                }
            } else {
                ai.TargetPos = ai.ServeDefPos;
                ai.transform.forward = Vector3.right * -ai.CourtSide;
                ai.StateMachine.ChangeState(ai.ReceiveServeState);
            }
        }
    }
}
