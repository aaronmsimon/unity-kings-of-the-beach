using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class OffenseState : AIBaseState
    {
        public OffenseState(AI ai) : base(ai) { }

        private bool apexReached;
        private bool isSpiking;

        public override void Enter() {
            apexReached = false;
            isSpiking = false;

            ai.BallInfo.TargetSet += OnTargetSet;
            ai.BallHitGround += OnBallHitGround;
            ai.BallInfo.ApexReached += OnApexReached;
        }

        public override void Exit() {
            ai.BallInfo.TargetSet -= OnTargetSet;
            ai.BallHitGround -= OnBallHitGround;
            ai.BallInfo.ApexReached -= OnApexReached;
        }

        public override void Update() {
            if (ai.BallInfo.lastPlayerToHit != ai && ai.BallInfo.lastPlayerToHit != null) {
                if (Vector3.Distance(ai.transform.position, ai.BallInfo.TargetPos) > ai.Skills.TargetLockDistance) {
                    ai.MoveDir = (ai.BallInfo.TargetPos - ai.transform.position).normalized;
                } else {
                    ai.transform.position = ai.BallInfo.TargetPos;
                    ai.MoveDir = Vector3.zero;
                    if (ai.BallInfo.HitsForTeam == 2 && !isSpiking) {
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
                    Debug.Log($"ball height at spike is {ai.BallInfo.Position.y}");
                    Vector3 targetPos = new Vector3(Random.Range(2, 8.5f) * -ai.CourtSide, 0, Random.Range(-4.5f, 4.5f));
                    ai.BallInfo.SetSpikeTarget(targetPos, Random.Range(0.5f, 1f), ai);
                }
            }
        }

        private void TrySpike() {
            float spikeRangeH = 1;
            if (Vector3.Distance(new Vector3(ai.transform.position.x, 0, ai.transform.position.z), new Vector3(ai.BallInfo.Position.x, 0, ai.BallInfo.Position.z)) <= spikeRangeH) {
                float spikeRangeV = Random.Range(4.75f, 5.25f);
                if (apexReached && ai.BallInfo.Position.y <= spikeRangeV) {
                    Debug.Log($"height when starting jumping: {spikeRangeV}");
                    ai.PerformJump();
                    isSpiking = true;
                }
            }
        }

        private void OnApexReached() {
            apexReached = true;
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
