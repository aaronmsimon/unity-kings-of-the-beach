using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class DigReadyState : AIBaseState
    {
        public DigReadyState(AI ai) : base(ai) { }

        private float reactionTime;
        private float spikeTime;
        private bool isSpiking;

        public override void Enter() {
            reactionTime = ai.Skills.ReactionTime;
            // Expensive operation (square root) so running this just once this state is entered
            spikeTime = GetTimeForSpike(ai.ReachHeight, ai.BallInfo.Height, ai.BallInfo.StartPos.y, ai.BallInfo.TargetPos.y, ai.BallInfo.Duration);
            isSpiking = false;
        }

        public override void Update() {
            reactionTime -= Time.deltaTime;

            if (reactionTime < 0) {
                if ((ai.transform.position - ai.BallInfo.TargetPos).sqrMagnitude > ai.Skills.TargetLockDistance * ai.Skills.TargetLockDistance) {
                    ai.TargetPos = ai.BallInfo.TargetPos;
                } else {
                    if (ai.BallInfo.HitsForTeam == 2 && !isSpiking) {
                        TrySpike();
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
            float jumpDuration = ai.JumpFrames / ai.AnimationFrameRate;
            float spikeDuration = ai.SpikeFrames / ai.AnimationFrameRate;
            if (ai.BallInfo.TimeSinceLastHit >= spikeTime - jumpDuration - spikeDuration / 2) {
                ai.PerformJump();
                isSpiking = true;
            }
        }

        private float GetTimeForSpike(float spikePos, float height, float start, float end, float duration)
        {
            float a = -4 * height;
            float b = 4 * height - start + end;
            float c = start - spikePos;

            float discriminant = b * b - 4 * a * c;

            if (discriminant >= 0)
            {
                float sqrtDiscriminant = Mathf.Sqrt(discriminant);
                float t1 = (-b + sqrtDiscriminant) / (2 * a);
                float t2 = (-b - sqrtDiscriminant) / (2 * a);

                if (t1 >= 0.5f && t1 <= 1)
                    return t1 * duration;
                else if (t2 >= 0.5f && t2 <= 1)
                    return t2 * duration;
            }

            Debug.LogError("No real solution for the given verticalPosition.");
            return -1;
        }
    }
}
