using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class OffenseState : AIBaseState
    {
        public OffenseState(AI ai) : base(ai) { }

        public override void Enter() {
            ai.BallInfo.TargetSet += OnTargetSet;
            ai.BallHitGround += OnBallHitGround;
        }

        public override void Exit() {
            ai.BallInfo.TargetSet -= OnTargetSet;
            ai.BallHitGround -= OnBallHitGround;
        }

        public override void Update() {
            if (ai.BallInfo.lastPlayerToHit != ai && ai.BallInfo.lastPlayerToHit != null) {
                if (Vector3.Distance(ai.transform.position, ai.BallInfo.TargetPos) > ai.Skills.TargetLockDistance) {
                    ai.MoveDir = (ai.BallInfo.TargetPos - ai.transform.position).normalized;
                } else {
                    ai.transform.position = ai.BallInfo.TargetPos;
                    ai.MoveDir = Vector3.zero;
                }
            } else {
                // move to offensive position
            }
        }

        public override void OnTriggerEnter(Collider other) {
            if (ai.Ball != null) {
                if (ai.BallInfo.HitsForTeam < 2) {
                    ai.Pass();
                } else {
                    // shot
                    ai.Shoot();
                }
            }
        }

        private void OnTargetSet() {
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) == -ai.CourtSide) {
                ai.StateMachine.ChangeState(ai.DefenseState);
            }
        }

        private void OnBallHitGround() {
            ai.StateMachine.ChangeState(ai.PostPointState);
        }
    }
}
