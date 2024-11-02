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
                        ai.Spike(CalculateSpikeTarget());
                        ai.StateMachine.ChangeState(ai.DefenseState);
                        break;
                    default:
                        Debug.Log("Invalid number of hits, perhaps point for other team.");
                        break;
                }
            }
        }

        private Vector3 CalculatePassTarget() {
            Athlete teammate = ai.MatchInfo.GetTeammate(ai);
            Vector2 teammatePos = new Vector2(teammate.transform.position.x, teammate.transform.position.z);
            Vector2 aimLocation = ai.BallInfo.SkillValues.AdjustedPassLocation(teammatePos, ai.Skills.PassAccuracy / 10);
            return new Vector3(aimLocation.x, 0f, aimLocation.y);
        }

        private Vector3 CalculateSpikeTarget() {
            return new Vector3(Random.Range(3.5f, 8.5f) * -ai.CourtSide, 0, Random.Range(-4.5f, 4.5f));
        }

        private void TrySpike() {
            float jumpDuration = ai.JumpFrames / ai.AnimationFrameRate;
            float spikeDuration = ai.SpikeFrames / ai.AnimationFrameRate;
            if (ai.BallInfo.TimeSinceLastHit >= spikeTime - jumpDuration - spikeDuration / 2 && spikeTime >= 0) {
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

        private void CalculateAttackZones() {
            // Vector3 deepOpponent = (Mathf.Max(Mathf.Abs(opponent1.transform.position.x), Mathf.Abs(opponent2.transform.position.x)) == Mathf.Abs(opponent1.transform.position.x) ? opponent1 : opponent2).transform.position;
            // Vector3 shallowOpponent = (Mathf.Max(Mathf.Abs(opponent1.transform.position.x), Mathf.Abs(opponent2.transform.position.x)) == Mathf.Abs(opponent1.transform.position.x) ? opponent2 : opponent1).transform.position;

            // // Deep Zone
            // Vector2 deepZonePos = new Vector2(
            //     ((ai.CourtSideLength - Mathf.Abs(shallowOpponent.x)) / 2 + Mathf.Abs(shallowOpponent.x)) * Mathf.Sign(deepOpponent.x),
            //     ((Mathf.Abs(deepOpponent.z) + 4) / 2 - Mathf.Abs(deepOpponent.z)) * -Mathf.Sign(deepOpponent.z)
            // );
            // Vector2 deepZoneSize = new Vector2(
            //     ai.CourtSideLength - shallowOpponent.x,
            //     Mathf.Abs(deepOpponent.z) + 4
            // );
            // Helpers.DrawTargetZone(deepZonePos, deepZoneSize, deepZoneColor, true);

            // // Shallow Zone
            // Vector2 shallowZonePos = new Vector2(
            //     Mathf.Abs(deepOpponent.x) / 2 * Mathf.Sign(deepOpponent.x),
            //     ((Mathf.Abs(shallowOpponent.z) + 4) / 2 - Mathf.Abs(shallowOpponent.z)) * -Mathf.Sign(shallowOpponent.z)
            // );
            // Vector2 shallowZoneSize = new Vector2(
            //     Mathf.Abs(deepOpponent.x),
            //     Mathf.Abs(shallowOpponent.z) + 4
            // );
            // Helpers.DrawTargetZone(shallowZonePos, shallowZoneSize, shallowZoneColor, true);
        }
    }
}
