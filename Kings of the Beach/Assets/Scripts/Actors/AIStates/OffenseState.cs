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
                if (Vector3.Distance(ai.transform.position, ai.AdjustedTargetPos) > ai.Skills.TargetLockDistance) {
                    ai.MoveDir = (ai.AdjustedTargetPos - ai.transform.position).normalized;
                } else {
                    ai.transform.position = ai.AdjustedTargetPos;
                    ai.MoveDir = Vector3.zero;
                    if (ai.BallInfo.HitsForTeam == 2) {
                        TrySpike();
                    }
                }
            } else {
                // move to offensive position
                ai.MoveDir = Vector3.zero; // temp
            }
        }

        public override void OnTriggerEnter(Collider other) {
            if (ai.Ball != null) {
                if (ai.BallInfo.HitsForTeam < 2) {
                    ai.Pass();
                } else {
                    // shot
                }
            }
        }

        private void TrySpike() {
            float spikeRangeV = 3;
            float spikeRangeH = 0.5f;
            if (Vector3.Distance(new Vector3(ai.transform.position.x, 0, ai.transform.position.z), new Vector3(ai.BallInfo.Position.x, 0, ai.BallInfo.Position.z)) <= spikeRangeH) {
                if (ai.BallInfo.Position.y <= ai.SpikeOrigin.y + spikeRangeV) {
                    Vector3 targetPos = new Vector3(Random.Range(2, 8.5f) * -ai.CourtSide, 0, Random.Range(-4.5f, 4.5f));
                    ai.BallInfo.SetSpikeTarget(targetPos, Random.Range(0.5f, 1f), ai);
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
