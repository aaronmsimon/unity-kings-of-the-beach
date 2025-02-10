using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class ServeState : AIBaseState
    {
        public ServeState(AI ai) : base(ai) { }

        private float timeUntilServe;
        private float baseTime = 1f;
        private float randomOffsetTime = 0.5f;
        private bool changeToDefenseState;
        private float timeUntilDefense = 1f;

        private float nudgePower = 0.1f;
        private float nudgeHeight = 0.1f;

        public override void Enter() {
            timeUntilServe = baseTime + Random.Range(-randomOffsetTime, randomOffsetTime);

            changeToDefenseState = false;
        }

        public override void Update() {
            if (!changeToDefenseState) {
                // Face direction of serve
                ai.transform.rotation = Quaternion.LookRotation(Vector3.right * -ai.CourtSide);

                timeUntilServe -= Time.deltaTime;

                if (timeUntilServe < 0) {
                    changeToDefenseState = true;
                    AimServe();
                    ai.BallInfo.SetServeTarget();
                }
            } else {
                timeUntilDefense -= Time.deltaTime;
                if (timeUntilDefense < 0) {
                    ai.StateMachine.ChangeState(ai.DefenseState);
                }
            }
        }

        private void AimServe() {
            bool commitServe = false;
            Vector3 aimPoint = new Vector3(0, Random.Range(2.25f, 5), Random.Range(-2, 2));
            Vector3 adjustedAimPoint = ai.BallInfo.SkillValues.AdjustedServeDirection(aimPoint, ai.Skills.Serving);
            float servePower = Random.Range(0.65f, 1);

            while (!commitServe) {
                // Debug.Log($"ValidServe({adjustedAimPoint}, {servePower})");
                bool validServe = ai.BallInfo.ValidServe(adjustedAimPoint, servePower, ai);
                float randValue = Random.value * ai.BallInfo.SkillValues.ServeAccuracy.max;
                float serveSkill = ai.BallInfo.SkillValues.SkillToValue(ai.Skills.Serving, ai.BallInfo.SkillValues.ServeAccuracy);
                // string debugMsg = $"Valid Serve: {validServe}, Skill Check: {randValue} > {serveSkill} [{randValue > serveSkill}]";
                if (validServe || randValue > serveSkill) {
                    // Debug.Log($"{debugMsg} Passed, serving");
                    commitServe = true;
                } else {
                    // Debug.Log($"{debugMsg} Failed, so trying again");
                    adjustedAimPoint += new Vector3(0f, nudgeHeight, 0f);
                    servePower += nudgePower;
                }
            }
        }
    }
}
