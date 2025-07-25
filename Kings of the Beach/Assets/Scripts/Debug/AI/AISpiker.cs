using System.Collections.Generic;
using UnityEngine;
using KotB.Actors;
using KotB.Items;

namespace KotB.Testing
{
    public class AISpiker : AI
    {
        private float reactionTime;
        private float spikeTime;

        private bool isSpiking = false;
        private Vector2[] attackZoneCenters = new Vector2[2];
        private Vector2[] attackZoneSizes = new Vector2[2];

        protected override void Start() {
            base.Start();

            TargetPos = transform.position;
            reactionTime = skills.ReactionTime;
            spikeTime = GetTimeToContactHeight(ReachHeight, BallInfo.Height, BallInfo.StartPos.y, BallInfo.TargetPos.y, BallInfo.Duration);
            FaceOpponent();
            Reset();
        }

        private void OnEnable() {
            SpikeTrigger.Triggered += OnSpikeTriggered;
        }

        private void OnDisable() {
            SpikeTrigger.Triggered -= OnSpikeTriggered;
        }

        protected override void Update() {
            base.Update();

            reactionTime -= Time.deltaTime;

            if (reactionTime < 0) {
                if ((transform.position - BallInfo.TargetPos).sqrMagnitude > Skills.TargetLockDistance * Skills.TargetLockDistance && !BallInfo.LockedOn) {
                    Vector2 lockTowardsTarget = LockTowardsTarget();
                    Vector3 lockPos = new Vector3(lockTowardsTarget.x, 0.01f, lockTowardsTarget.y);
                    TargetPos = lockPos;
                    BallInfo.LockedOn = true;
                } else {
                    if (BallInfo.HitsForTeam == 2 && !isSpiking) {
                        TrySpike();
                    }
                }
            }
        }

        public void Reset() {
            ballInfo.HitsForTeam = 1;
            isSpiking = false;
        }

        private void TrySpike() {
            float jumpDuration = JumpFrames / AnimationFrameRate;
            float spikeDuration = ActionFrames / AnimationFrameRate;
            float optimalTime = spikeTime - jumpDuration - spikeDuration / 2;

            // Timing window based on skill
            float timingWindow = BallInfo.SkillValues.SkillToValue(Skills.SpikeSkill, BallInfo.SkillValues.SpikeTimingWindow);
            float minTime = optimalTime - timingWindow;
            float maxTime = optimalTime + timingWindow;

            if (BallInfo.TimeSinceLastHit >= minTime && BallInfo.TimeSinceLastHit <= maxTime && spikeTime >= 0) {
                PerformJump();
                isSpiking = true;

                // Calculate timing penalty (similar to human system)
                float actualTimingError = BallInfo.TimeSinceLastHit - optimalTime;
                SpikeSpeedPenalty = actualTimingError * timingWindow;
                Debug.Log($"actualTimingError: {BallInfo.TimeSinceLastHit} - {optimalTime} = {actualTimingError} x window {timingWindow} = {actualTimingError * timingWindow}");
            }
        }

        private Vector3 CalculateSpikeTarget() {
            Vector2 xRange, zRange;

            if (MatchInfo.GetOpposingTeam(this) != null) {
                CalculateAttackZones();
                int useZoneIndex;
                if (Mathf.Abs(transform.position.x) <= 2) {
                    useZoneIndex = LargestAttackZoneIndex();
                } else {
                    useZoneIndex = 0;
                }
                xRange = new Vector2(attackZoneCenters[useZoneIndex].x - attackZoneSizes[useZoneIndex].x / 2, attackZoneCenters[useZoneIndex].x + attackZoneSizes[useZoneIndex].x / 2);
                zRange = new Vector2(attackZoneCenters[useZoneIndex].y - attackZoneSizes[useZoneIndex].y / 2, attackZoneCenters[useZoneIndex].y + attackZoneSizes[useZoneIndex].y / 2);
            } else {
                xRange = new Vector2(0, CourtSideLength * -CourtSide);
                zRange = new Vector2(-CourtSideLength / 2, CourtSideLength / 2);
            }

            Vector3 target = new Vector3(Random.Range(xRange.x, xRange.y), 0, Random.Range(zRange.x, zRange.y));
            return target;
        }

        private void CalculateAttackZones() {
            List<Athlete> opponents = MatchInfo.GetOpposingTeam(this).Athletes;
            Vector3 deepOpponent = (Mathf.Max(Mathf.Abs(opponents[0].transform.position.x), Mathf.Abs(opponents[1].transform.position.x)) == Mathf.Abs(opponents[0].transform.position.x) ? opponents[0] : opponents[1]).transform.position;
            Vector3 shallowOpponent = (Mathf.Max(Mathf.Abs(opponents[0].transform.position.x), Mathf.Abs(opponents[1].transform.position.x)) == Mathf.Abs(opponents[0].transform.position.x) ? opponents[1] : opponents[0]).transform.position;

            float rangeDecrease = BallInfo.SkillValues.WeightedDecrease(Skills.SpikeSkill);

            // Deep Zone
            attackZoneCenters[0] = new Vector2(
                ((CourtSideLength - Mathf.Abs(shallowOpponent.x)) / 2 + Mathf.Abs(shallowOpponent.x)) * Mathf.Sign(deepOpponent.x),
                ((Mathf.Abs(deepOpponent.z) + 4) / 2 - Mathf.Abs(deepOpponent.z)) * -Mathf.Sign(deepOpponent.z)
            );
            attackZoneSizes[0] = new Vector2(
                (CourtSideLength - Mathf.Abs(shallowOpponent.x)) * (1 - rangeDecrease),
                (Mathf.Abs(deepOpponent.z) + 4) * (1 - rangeDecrease)
            );

            // Shallow Zone
            attackZoneCenters[1] = new Vector2(
                Mathf.Abs(deepOpponent.x) / 2 * Mathf.Sign(deepOpponent.x),
                ((Mathf.Abs(shallowOpponent.z) + 4) / 2 - Mathf.Abs(shallowOpponent.z)) * -Mathf.Sign(shallowOpponent.z)
            );
            attackZoneSizes[1] = new Vector2(
                Mathf.Abs(deepOpponent.x) * (1 - rangeDecrease),
                (Mathf.Abs(shallowOpponent.z) + 4) * (1 - rangeDecrease)
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

        private void OnSpikeTriggered(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                Feint = false;
                Spike(CalculateSpikeTarget());
                DigToDefensePredicate.Trigger();
            }
        }
    }
}
