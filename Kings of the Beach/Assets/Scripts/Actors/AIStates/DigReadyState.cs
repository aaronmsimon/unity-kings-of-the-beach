using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class DigReadyState : AIBaseState
    {
        public DigReadyState(AI ai) : base(ai) { }

        private float reactionTime;

        public override void Enter() {
            reactionTime = ai.Skills.ReactionTime;

            ai.BallHitGround += OnBallHitGround;
        }

        public override void Exit() {
            ai.BallHitGround -= OnBallHitGround;
        }

        public override void Update() {
            reactionTime -= Time.deltaTime;

            float myDistToBall = (ai.BallInfo.TargetPos - ai.transform.position).sqrMagnitude;
            float teammateDistToBall = (ai.BallInfo.TargetPos - ai.Teammate.transform.position).sqrMagnitude;

            if (myDistToBall < teammateDistToBall && reactionTime < 0) {
                if (Vector3.Distance(ai.transform.position, ai.AdjustedTargetPos) > ai.Skills.TargetLockDistance) {
                    ai.MoveDir = (ai.AdjustedTargetPos - ai.transform.position).normalized;
                } else {
                    ai.transform.position = ai.AdjustedTargetPos;
                    ai.MoveDir = Vector3.zero;
                }
            }

            CheckTransition();
        }

        public override void OnTriggerEnter(Collider other) {
            if (ai.Ball != null) {
                ai.Pass();
            }
        }

        private void CheckTransition() {
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide && ai.BallInfo.HitsForTeam == 1) {
                ai.StateMachine.ChangeState(ai.OffenseState);
            }
        }

        private void OnBallHitGround() {
            ai.StateMachine.ChangeState(ai.PostPointState);
        }
    }
}
