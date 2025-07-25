using System.Collections.Generic;
using UnityEngine;
using KotB.Actors;
using KotB.Items;

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
            spikeTime = ai.GetTimeToContactHeight(ai.ReachHeight, ai.BallInfo.Height, ai.BallInfo.StartPos.y, ai.BallInfo.TargetPos.y, ai.BallInfo.Duration);
            isSpiking = false;

            ai.BodyTrigger.Triggered += OnBodyTriggered;
            ai.SpikeTrigger.Triggered += OnSpikeTriggered;
            ai.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ai.BodyTrigger.Triggered -= OnBodyTriggered;
            ai.SpikeTrigger.Triggered -= OnSpikeTriggered;
            ai.BallInfo.TargetSet -= OnTargetSet;
        }

        public override void Update() {
            reactionTime -= Time.deltaTime;

            if (reactionTime < 0) {
                if ((ai.transform.position - ai.BallInfo.TargetPos).sqrMagnitude > ai.Skills.TargetLockDistance * ai.Skills.TargetLockDistance && !ai.BallInfo.LockedOn) {
                    Vector2 lockTowardsTarget = ai.LockTowardsTarget();
                    Vector3 lockPos = new Vector3(lockTowardsTarget.x, 0.01f, lockTowardsTarget.y);
                    ai.TargetPos = lockPos;
                    ai.BallInfo.LockedOn = true;
                } else {
                    if (ai.BallInfo.HitsForTeam == 2 && !isSpiking) {
                        TrySpike();
                    }
                }
            }
        }

        public void OnBodyTriggered(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                switch (ai.BallInfo.HitsForTeam) {
                    case 0:
                        ai.Pass(ai.CalculatePassTarget(ai.MatchInfo.GetTeammate(ai)), 7, 1.75f, PassType.Bump);
                        ai.DigToOffensePredicate.Trigger();
                        break;
                    case 1:
                        ai.Pass(ai.CalculatePassTarget(ai.MatchInfo.GetTeammate(ai)), 7, 1.75f, PassType.Set);
                        ai.DigToDefensePredicate.Trigger();
                        break;
                    case 2:
                        Vector3 bumpTarget = CalculateSpikeTarget();
                        ai.Pass(bumpTarget, 7, 1.75f, PassType.Bump);
                        ai.DigToDefensePredicate.Trigger();
                        break;
                    default:
                        Debug.Log("Invalid number of hits, perhaps point for other team.");
                        break;
                }
            }
        }

        private void OnSpikeTriggered(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                ConsiderSpikeFeint();
                ai.Spike(CalculateSpikeTarget());
                ai.DigToDefensePredicate.Trigger();
            }
        }

        private Vector3 CalculateSpikeTarget() {
            Vector2 xRange, zRange;

            if (ai.MatchInfo.GetOpposingTeam(ai) != null) {
                CalculateAttackZones();
                int useZoneIndex;
                if (Mathf.Abs(ai.transform.position.x) <= 2) {
                    useZoneIndex = LargestAttackZoneIndex();
                } else {
                    useZoneIndex = 0;
                }
                xRange = new Vector2(attackZoneCenters[useZoneIndex].x - attackZoneSizes[useZoneIndex].x / 2, attackZoneCenters[useZoneIndex].x + attackZoneSizes[useZoneIndex].x / 2);
                zRange = new Vector2(attackZoneCenters[useZoneIndex].y - attackZoneSizes[useZoneIndex].y / 2, attackZoneCenters[useZoneIndex].y + attackZoneSizes[useZoneIndex].y / 2);
            } else {
                xRange = new Vector2(0, ai.CourtSideLength * -ai.CourtSide);
                zRange = new Vector2(-ai.CourtSideLength / 2, ai.CourtSideLength / 2);
            }

            Vector3 target = new Vector3(Random.Range(xRange.x, xRange.y), 0, Random.Range(zRange.x, zRange.y));
            return target;
        }

        private void TrySpike() {
            float jumpDuration = ai.JumpFrames / ai.AnimationFrameRate;
            float spikeDuration = ai.ActionFrames / ai.AnimationFrameRate;
            float optimalTime = spikeTime - jumpDuration - spikeDuration / 2;

            // Timing window based on skill
            float timingWindow = ai.BallInfo.SkillValues.SkillToValue(ai.Skills.SpikeSkill, ai.BallInfo.SkillValues.SpikeTimingWindow);
            float minTime = optimalTime - timingWindow;
            float maxTime = optimalTime + timingWindow;

            if (ai.BallInfo.TimeSinceLastHit >= minTime && ai.BallInfo.TimeSinceLastHit <= maxTime && spikeTime >= 0) {
                ai.PerformJump();
                isSpiking = true;

                // Calculate timing penalty (similar to human system)
                float actualTimingError = ai.BallInfo.TimeSinceLastHit - optimalTime;
                ai.SpikeSpeedPenalty = actualTimingError * timingWindow;
                Debug.Log($"actualTimingError: {ai.BallInfo.TimeSinceLastHit} - {optimalTime} = {actualTimingError} x window {timingWindow} = {actualTimingError * timingWindow}");

                // Timing penalty - match human system pattern
                // float normalizedTimingError = (ai.BallInfo.TimeSinceLastHit - optimalTime) / timingWindow; // Range: -1 to 1
                // Debug.Log($"normalizedTimingError: ({ai.BallInfo.TimeSinceLastHit} - {optimalTime}) / {timingWindow} = {normalizedTimingError} x window {timingWindow} = {Mathf.Abs(normalizedTimingError) * timingWindow}");
                // ai.SpikeSpeedPenalty = Mathf.Abs(normalizedTimingError) * timingWindow; // Should be smaller values

                // float timingError = Mathf.Abs(ai.BallInfo.TimeSinceLastHit - optimalTime);
                // ai.SpikeSpeedPenalty = Mathf.Clamp01(timingError / timingWindow);
            }
        }

        private void ConsiderSpikeFeint() {
            // Opponent present
            // Opponent's block skill (if applicable)
            // Spike success this game

            // Temp random
            ai.Feint = Random.Range(0f,1) > 0.9f;
        }

        private void CalculateAttackZones() {
            List<Athlete> opponents = ai.MatchInfo.GetOpposingTeam(ai).Athletes;
            Vector3 deepOpponent = (Mathf.Max(Mathf.Abs(opponents[0].transform.position.x), Mathf.Abs(opponents[1].transform.position.x)) == Mathf.Abs(opponents[0].transform.position.x) ? opponents[0] : opponents[1]).transform.position;
            Vector3 shallowOpponent = (Mathf.Max(Mathf.Abs(opponents[0].transform.position.x), Mathf.Abs(opponents[1].transform.position.x)) == Mathf.Abs(opponents[0].transform.position.x) ? opponents[1] : opponents[0]).transform.position;

            float rangeDecrease = ai.BallInfo.SkillValues.WeightedDecrease(ai.Skills.SpikeSkill);

            // Deep Zone
            attackZoneCenters[0] = new Vector2(
                ((ai.CourtSideLength - Mathf.Abs(shallowOpponent.x)) / 2 + Mathf.Abs(shallowOpponent.x)) * Mathf.Sign(deepOpponent.x),
                ((Mathf.Abs(deepOpponent.z) + 4) / 2 - Mathf.Abs(deepOpponent.z)) * -Mathf.Sign(deepOpponent.z)
            );
            attackZoneSizes[0] = new Vector2(
                (ai.CourtSideLength - Mathf.Abs(shallowOpponent.x)) * (1 - rangeDecrease),
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

        private void OnTargetSet() {
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) != ai.CourtSide) {
                ai.DigToDefensePredicate.Trigger();
            }
        }
    }
}
