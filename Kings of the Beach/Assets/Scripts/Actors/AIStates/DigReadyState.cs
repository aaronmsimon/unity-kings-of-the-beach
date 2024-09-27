using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class DigReadyState : AIBaseState
    {
        public DigReadyState(AI ai) : base(ai) { }

        public override void Update() {
            float myDistToBall = (ai.BallInfo.TargetPos - ai.transform.position).sqrMagnitude;
            float teammateDistToBall = (ai.BallInfo.TargetPos - ai.Teammate.transform.position).sqrMagnitude;

            if (myDistToBall < teammateDistToBall) {
                if (Vector3.Distance(ai.transform.position, ai.BallInfo.TargetPos) > ai.Skills.TargetLockDistance) {
                    ai.MoveDir = (ai.BallInfo.TargetPos - ai.transform.position).normalized;
                } else {
                    ai.MoveDir = Vector3.zero;
                }
            }

            CheckTransition();
        }

        public override void OnTriggerEnter(Collider other) {
            ai.Pass();
        }

        private void CheckTransition() {
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide && ai.BallInfo.HitsForTeam == 1) {
                ai.StateMachine.ChangeState(ai.OffenseState);
            }
        }
    }
}
