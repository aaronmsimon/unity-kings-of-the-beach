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
            if (ai.Teammate != null) {
                defenseZPos = ai.Skills.DefensePos.y * (ai.Teammate.GetComponent<Player>() != null || ai.Teammate.Skills.PlayerPosition == PositionType.Blocker ? -Mathf.Sign(ai.Teammate.Skills.Position.z) : 1);
            } else {
                defenseZPos = 0;
            }

            return new Vector3(ai.Skills.DefensePos.x * ai.CourtSide, 0.01f, defenseZPos);
        }

        private void OnBallServed() {
            ai.StateMachine.ChangeState(ai.DefenseState);
        }

        private void OnReachedTargetPos() {
            ai.transform.forward = Vector3.right * -ai.CourtSide;
        }
    }
}
