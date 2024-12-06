using System.Collections.Generic;
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
        private Vector2[] attackZoneCenters = new Vector2[2];
        private Vector2[] attackZoneSizes = new Vector2[2];

        public override void Enter() {
            reactionTime = ai.Skills.ReactionTime;
            // Expensive operation (square root) so running this just once this state is entered
            spikeTime = GetTimeForSpike(ai.ReachHeight, ai.BallInfo.Height, ai.BallInfo.StartPos.y, ai.BallInfo.TargetPos.y, ai.BallInfo.Duration);
            isSpiking = false;

            ai.BallInfo.BallChangePossession += OnBallChangePosession;
        }

        public override void Exit() {
            ai.BallInfo.BallChangePossession -= OnBallChangePosession;
        }

        public override void Update() {
            reactionTime -= Time.deltaTime;

            if (reactionTime < 0) {
                if ((ai.transform.position - ai.BallInfo.TargetPos).sqrMagnitude > ai.Skills.TargetLockDistance * ai.Skills.TargetLockDistance && !ai.BallInfo.LockedOn) {
                    ai.TargetPos = ai.BallInfo.TargetPos;
                } else {
                    ai.BallInfo.LockedOn = true;
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
                        ai.Pass(CalculatePassTarget(), 7, 1.75f);
                        ai.StateMachine.ChangeState(ai.OffenseState);
                        break;
                    case 1:
                        ai.Pass(CalculatePassTarget(), 7, 1.75f);
                        ai.StateMachine.ChangeState(ai.DefenseState);
                        break;
                    case 2:
                        ConsiderSpikeFeint();
                        Vector3 spikeTarget = CalculateSpikeTarget();
                        if (!ai.Feint) {
                            ai.Spike(spikeTarget);
                        } else {
                            ai.SpikeFeint(spikeTarget);
                        }
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
            Vector2 xRange, zRange;

            if (ai.MatchInfo.GetOpposingTeam(ai) != null) {
                CalculateAttackZones();
                int largestZoneIndex = LargestAttackZoneIndex();
                xRange = new Vector2(attackZoneCenters[largestZoneIndex].x - attackZoneSizes[largestZoneIndex].x / 2, attackZoneCenters[largestZoneIndex].x + attackZoneSizes[largestZoneIndex].x / 2);
                zRange = new Vector2(attackZoneCenters[largestZoneIndex].y - attackZoneSizes[largestZoneIndex].y / 2, attackZoneCenters[largestZoneIndex].y + attackZoneSizes[largestZoneIndex].y / 2);
            } else {
                xRange = new Vector2(0, ai.CourtSideLength * -ai.CourtSide);
                zRange = new Vector2(-ai.CourtSideLength / 2, ai.CourtSideLength / 2);
            }

            Vector3 target = new Vector3(Random.Range(xRange.x, xRange.y), 0, Random.Range(zRange.x, zRange.y));
            return target;
        }

        private void TrySpike() {
            float jumpDuration = ai.JumpFrames / ai.AnimationFrameRate;
            float spikeDuration = ai.SpikeFrames / ai.AnimationFrameRate;
            if (ai.BallInfo.TimeSinceLastHit >= spikeTime - jumpDuration - spikeDuration / 2 && spikeTime >= 0) {
                ai.PerformJump();
                isSpiking = true;
            }
        }

        private void ConsiderSpikeFeint() {
            // Opponent present
            // Opponent's block skill (if applicable)
            // Spike success this game

            // Temp random
            ai.Feint = Random.Range(0f,1) > 0.9f;
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
            List<Athlete> opponents = ai.MatchInfo.GetOpposingTeam(ai).Athletes;
            Vector3 deepOpponent = (Mathf.Max(Mathf.Abs(opponents[0].transform.position.x), Mathf.Abs(opponents[1].transform.position.x)) == Mathf.Abs(opponents[0].transform.position.x) ? opponents[0] : opponents[1]).transform.position;
            Vector3 shallowOpponent = (Mathf.Max(Mathf.Abs(opponents[0].transform.position.x), Mathf.Abs(opponents[1].transform.position.x)) == Mathf.Abs(opponents[0].transform.position.x) ? opponents[1] : opponents[0]).transform.position;

            // Deep Zone
            attackZoneCenters[0] = new Vector2(
                ((ai.CourtSideLength - Mathf.Abs(shallowOpponent.x)) / 2 + Mathf.Abs(shallowOpponent.x)) * Mathf.Sign(deepOpponent.x),
                ((Mathf.Abs(deepOpponent.z) + 4) / 2 - Mathf.Abs(deepOpponent.z)) * -Mathf.Sign(deepOpponent.z)
            );
            attackZoneSizes[0] = new Vector2(
                ai.CourtSideLength -Mathf.Abs(shallowOpponent.x),
                Mathf.Abs(deepOpponent.z) + 4
            );

            // Shallow Zone
            attackZoneCenters[1] = new Vector2(
                Mathf.Abs(deepOpponent.x) / 2 * Mathf.Sign(deepOpponent.x),
                ((Mathf.Abs(shallowOpponent.z) + 4) / 2 - Mathf.Abs(shallowOpponent.z)) * -Mathf.Sign(shallowOpponent.z)
            );
            attackZoneSizes[1] = new Vector2(
                Mathf.Abs(deepOpponent.x),
                Mathf.Abs(shallowOpponent.z) + 4
            );
        }

        private int LargestAttackZoneIndex() {
            float largestArea = -1;
            int index = -1;

            for (int i = 0; i < attackZoneSizes.Length; i++)
            {
                float thisArea = attackZoneSizes[i].x * attackZoneSizes[i].y;
                if (thisArea > largestArea) {
                    largestArea = thisArea;
                    index = i;
                }                
            }

            return index;
        }

        private void OnBallChangePosession() {
            if (ai.BallInfo.Possession == ai.CourtSide) {
                ai.StateMachine.ChangeState(ai.DefenseState);
            }
        }
    }
}
