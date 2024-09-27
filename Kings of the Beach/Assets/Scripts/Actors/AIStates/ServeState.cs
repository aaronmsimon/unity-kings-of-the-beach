using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class ServeState : AIBaseState
    {
        public ServeState(AI ai) : base(ai) { }

        private float timeUntilServe;
        private float baseTime = 2f;
        private float randomOffsetTime = 0.5f;

        public override void Enter() {
            timeUntilServe = baseTime + Random.Range(-randomOffsetTime, randomOffsetTime);
            ai.BallHitGround += OnBallHitGround;
        }

        public override void Exit() {
            ai.BallHitGround -= OnBallHitGround;
        }

        public override void Update() {
            timeUntilServe -= Time.deltaTime;

            if (timeUntilServe < 0) {
                float targetX = Random.Range(0, ai.CourtSideLength) * ai.CourtSide;
                float targetZ = Random.Range(-ai.CourtSideLength / 2, ai.CourtSideLength / 2);
                Vector3 targetPos = new Vector3(targetX, 0, targetZ);
                float height = 3;
                ai.BallInfo.SetPassTarget(targetPos, height, 1.5f, ai);
                ai.BallInfo.BallServedEvent();
                ai.StateMachine.ChangeState(ai.DefenseState);
            }
        }

        private void OnBallHitGround() {
            ai.StateMachine.ChangeState(ai.PostPointState);
        }
    }
}
