using System.Collections;
using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class ServeState : AIBaseState
    {
        public ServeState(AI ai) : base(ai) { }

        private Vector3 servePos;
        private float servePosThreshold = 0.25f;
        private float timeUntilServe;
        private float baseTime = 2f;
        private float randomOffsetTime = 0.5f;
        private bool changeToDefenseState;
        private float timeUntilDefense;

        public override void Enter() {
            ai.transform.position = new Vector3((ai.CourtSideLength + ai.transform.localScale.x * .5f) * ai.CourtSide, 0.01f, 0f);
            servePos = new Vector3(ai.transform.position.x, ai.transform.position.y, Random.Range(-ai.CourtSideLength / 2, ai.CourtSideLength / 2));

            timeUntilServe = baseTime + Random.Range(-randomOffsetTime, randomOffsetTime);

            changeToDefenseState = false;
        }

        public override void Update() {
            if (!changeToDefenseState) {
                if (Mathf.Abs(servePos.z - ai.transform.position.z) > servePosThreshold) {
                    ai.transform.position += (servePos - ai.transform.position).normalized * ai.Skills.MoveSpeed * Time.deltaTime;
                } else {
                    timeUntilServe -= Time.deltaTime;

                    if (timeUntilServe < 0) {
                        Vector3 aimPoint = new Vector3(0, Random.Range(2.25f, 5), Random.Range(-2, 2));
                        Vector3 adjustedAimPoint = ai.BallInfo.SkillValues.AdjustedServeDirection(aimPoint, ai.Skills.Serving);
                        Debug.Log($"aim pos: {aimPoint} adjusted: {adjustedAimPoint}");
                        ai.BallInfo.SetServeTarget(adjustedAimPoint, Random.Range(0.65f, 1), ai);
                        ai.BallInfo.BallServedEvent();
                        changeToDefenseState = true;
                        timeUntilDefense = 1;
                    }
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
