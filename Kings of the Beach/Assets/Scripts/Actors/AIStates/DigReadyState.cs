using UnityEngine;
using KotB.Actors;
using KotB.StatePattern.MatchStates;

namespace KotB.StatePattern.AIStates
{
    public class DigReadyState : AIBaseState
    {
        public DigReadyState(AI ai) : base(ai) { }

        private float reactionTime;
        private bool apexReached;
        private bool isSpiking;

        public override void Enter() {
            reactionTime = ai.Skills.ReactionTime;
            apexReached = false;
            isSpiking = false;

            ai.BallHitGround += OnBallHitGround;
            ai.BallInfo.ApexReached += OnApexReached;
        }

        public override void Exit() {
            ai.BallHitGround -= OnBallHitGround;
            ai.BallInfo.ApexReached -= OnApexReached;
        }

        public override void Update() {
            reactionTime -= Time.deltaTime;

            if (reactionTime < 0) {
                if ((ai.transform.position - ai.BallInfo.TargetPos).sqrMagnitude > ai.Skills.TargetLockDistance * ai.Skills.TargetLockDistance) {
                    ai.MoveDir = (ai.BallInfo.TargetPos - ai.transform.position).normalized;
                } else {
                    if (ai.MatchInfo.CurrentState is InPlayState && ai.BallInfo.lastPlayerToHit != ai && Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide) {
                        ai.transform.position = ai.BallInfo.TargetPos;
                        ai.MoveDir = Vector3.zero;
                        if (ai.BallInfo.HitsForTeam == 2 && !isSpiking) {
                            TrySpike();
                        }
                    }
                }
            }
        }

        public override void OnTriggerEnter(Collider other) {
            if (ai.Ball != null) {
                switch (ai.BallInfo.HitsForTeam) {
                    case 0:
                        ai.Pass(CalculatePassTarget());
                        ai.StateMachine.ChangeState(ai.OffenseState);
                        break;
                    case 1:
                        ai.Pass(CalculatePassTarget());
                        ai.StateMachine.ChangeState(ai.DefenseState);
                        break;
                    case 2:
                        Spike();
                        ai.StateMachine.ChangeState(ai.DefenseState);
                        break;
                    default:
                        Debug.Log("Invalid number of hits, perhaps point for other team.");
                        break;
                }
            }
        }

        private Vector3 CalculatePassTarget() {
            Vector2 teammatePos = new Vector2(ai.Teammate.transform.position.x, ai.Teammate.transform.position.z);
            Vector2 aimLocation = ai.BallInfo.SkillValues.AdjustedPassLocation(teammatePos, ai.Skills.PassAccuracy / 10);
            return new Vector3(aimLocation.x, 0f, aimLocation.y);
        }

        private void Spike() {
            if (Mathf.Abs(ai.transform.position.x) <= 1.5f) {
                Vector3 targetPos = new Vector3(Random.Range(3.5f, 8.5f) * -ai.CourtSide, 0, Random.Range(-4.5f, 4.5f));
                ai.BallInfo.SetSpikeTarget(targetPos, Random.Range(0.5f, 1f), ai);
            } else {
                ai.BallInfo.SetServeTarget(new Vector3(0, Random.Range(2.5f, 4), Random.Range(ai.transform.position.z - 1.5f, ai.transform.position.z + 1.5f)), 0.5f, ai);
            }
        }

        private void TrySpike() {
            float spikeRangeH = 1;
            if (Vector3.Distance(new Vector3(ai.transform.position.x, 0, ai.transform.position.z), new Vector3(ai.BallInfo.Position.x, 0, ai.BallInfo.Position.z)) <= spikeRangeH) {
                float spikeRangeV = Random.Range(6, 7);
                if (apexReached && ai.BallInfo.Position.y <= spikeRangeV) {
                    ai.PerformJump();
                    isSpiking = true;
                }
            }
        }

        private void OnApexReached() {
            apexReached = true;
        }

        private void OnBallHitGround() {
            ai.StateMachine.ChangeState(ai.PostPointState);
            ai.MoveDir = Vector3.zero;
        }
    }
}
