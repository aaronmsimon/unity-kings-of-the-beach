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

        public override void Enter() {
            ai.transform.position = new Vector3((ai.CourtSideLength + ai.transform.localScale.x * .5f) * ai.CourtSide, 0.01f, 0f);
            servePos = new Vector3(ai.transform.position.x, ai.transform.position.y, Random.Range(-ai.CourtSideLength / 2, ai.CourtSideLength / 2));

            timeUntilServe = baseTime + Random.Range(-randomOffsetTime, randomOffsetTime);
            ai.BallHitGround += OnBallHitGround;
        }

        public override void Exit() {
            ai.BallHitGround -= OnBallHitGround;
        }

        public override void Update() {
            if (Mathf.Abs(servePos.z - ai.transform.position.z) > servePosThreshold) {
                ai.transform.position += (servePos - ai.transform.position).normalized * ai.Skills.MoveSpeed * Time.deltaTime;
            } else {
                timeUntilServe -= Time.deltaTime;

                if (timeUntilServe < 0) {
                    float targetX = Random.Range(0, ai.CourtSideLength) * -ai.CourtSide;
                    float targetZ = Random.Range(-ai.CourtSideLength / 2, ai.CourtSideLength / 2);
                    Vector3 targetPos = new Vector3(targetX, 0, targetZ);
                    float height = 3;
                    ai.BallInfo.SetPassTarget(targetPos, height, 1.5f, ai);
                    ai.BallInfo.BallServedEvent();
                    ai.StateMachine.ChangeState(ai.DefenseState);
                }
            }
        }

        private void OnBallHitGround() {
            ai.StateMachine.ChangeState(ai.PostPointState);
        }
    }
}
