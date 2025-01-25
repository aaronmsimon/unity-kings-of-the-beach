using UnityEngine;
using KotB.Actors;
using KotB.Match;

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
                    ai.StateMachine.ChangeState(ai.ServeState);
                } else {
                    ai.StateMachine.ChangeState(ai.NonServeState);
                }
            } else {
                TeamSO team = ai.MatchInfo.GetTeam(ai);
                Vector3 serveDefPos = new Vector3(ai.ReceiveServeXPos * ai.CourtSide, 0.01f, ai.Skills.DefensePos.y * (team.IsCaptain(ai) ? 1 : -1));
                ai.transform.position = serveDefPos;
                ai.transform.forward = Vector3.right * -ai.CourtSide;
                ai.StateMachine.ChangeState(ai.DefenseState);
            }
        }
    }
}
