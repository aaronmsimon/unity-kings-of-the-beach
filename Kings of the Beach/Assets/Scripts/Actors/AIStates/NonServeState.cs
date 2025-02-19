using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class NonServeState : AIBaseState
    {
        public NonServeState(AI ai) : base(ai) { }

        public override void Enter() {
            ai.transform.position = GetMyDefensivePosition();

            ai.BallInfo.BallServed += OnBallServed;
            ai.ReachedTargetPos += OnReachedTargetPos;
        }

        public override void Exit() {
            ai.BallInfo.BallServed -= OnBallServed;
            ai.ReachedTargetPos -= OnReachedTargetPos;
        }

        public override void Update() {
            ai.TargetPos = GetMyDefensivePosition();
        }

        private Vector3 GetMyDefensivePosition() {
            float defenseZPos;
            // If no teammate (debugging but potentially practice, too)
            Athlete teammate = ai.MatchInfo.GetTeammate(ai);
            if (teammate != null) {
                defenseZPos = ai.Skills.DefensePos.y * (teammate.GetComponent<Player>() != null || teammate.Skills.PlayerPosition == PositionType.Blocker ? -Mathf.Sign(teammate.Skills.Position.z) : 1);
            } else {
                defenseZPos = 0;
            }

            return new Vector3(ai.Skills.DefensePos.x * ai.CourtSide, 0.01f, defenseZPos);
        }

        private void OnBallServed() {
            ai.StateMachine.ChangeState(ai.DefenseState);
        }

        private void OnReachedTargetPos() {
            ai.FaceOpponent();
        }
    }
}
