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
            if (ai.MatchInfo.Server == ai) {
                ai.StateMachine.ChangeState(ai.ServeState);
            } else {
                ai.StateMachine.ChangeState(ai.NonServeState);
            }
        }
    }
}
