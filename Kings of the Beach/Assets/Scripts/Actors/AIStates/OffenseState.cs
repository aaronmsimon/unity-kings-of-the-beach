using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class OffenseState : AIBaseState
    {
        public OffenseState(AI ai) : base(ai) { }

        public override void Enter() {
            ai.BallInfo.BallChangePossession += TransitionToDefense;
        }

        public override void Exit() {
            ai.BallInfo.BallChangePossession -= TransitionToDefense;
        }

        public override void Update() {
            if (ai.BallInfo.lastPlayerToHit != ai && ai.BallInfo.lastPlayerToHit != null) {
                if (Vector3.Distance(ai.transform.position, ai.BallInfo.TargetPos) > ai.Skills.TargetLockDistance) {
                    ai.MoveDir = (ai.BallInfo.TargetPos - ai.transform.position).normalized;
                } else {
                    ai.MoveDir = Vector3.zero;
                }
            } else {
                // move to offensive position
            }
        }

        public override void OnTriggerEnter(Collider other) {
            if (ai.BallInfo.HitsForTeam < 2) {
                ai.Pass();
            } else {
                // shot
                // bumpTarget = new Vector3(Random.Range(0, courtSideLength * -courtSide), 0f, Random.Range(-courtSideLength / 2, courtSideLength / 2));
            }
        }

        private void TransitionToDefense() {
            ai.StateMachine.ChangeState(ai.DefenseState);
        }
    }
}
