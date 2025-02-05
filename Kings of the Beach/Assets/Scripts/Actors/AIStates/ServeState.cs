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
                    Vector3 aimPoint = new Vector3(0, Random.Range(2.25f, 5), Random.Range(-2, 2));
                    Vector3 adjustedAimPoint = ai.BallInfo.SkillValues.AdjustedServeDirection(aimPoint, ai.Skills.Serving);
                    changeToDefenseState = true;
                    ai.BallInfo.SetServeTarget(adjustedAimPoint, Random.Range(0.65f, 1), ai);
                }
            } else {
                timeUntilDefense -= Time.deltaTime;
                if (timeUntilDefense < 0) {
                    ai.StateMachine.ChangeState(ai.DefenseState);
                }
            }
        }
    }
}
