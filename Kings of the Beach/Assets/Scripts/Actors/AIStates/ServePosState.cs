using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class ServePosState : AIBaseState
    {
        public ServePosState(AI ai) : base(ai) { }

        private float timeUntilWalk;
        private float baseTime = 1f;
        private float randomOffsetTime = 0.25f;
        private Vector3 targetPos;

        public override void Enter() {
            ai.transform.position = new Vector3((ai.CourtSideLength + ai.transform.localScale.x * .5f) * ai.CourtSide, 0.01f, 0f);
            ai.TargetPos = ai.transform.position;
            ai.transform.rotation = Quaternion.LookRotation(Vector3.right * -ai.CourtSide);
            targetPos = new Vector3(ai.transform.position.x, ai.transform.position.y, Random.Range(-ai.CourtSideLength / 2, ai.CourtSideLength / 2));

            timeUntilWalk = baseTime + Random.Range(-randomOffsetTime, randomOffsetTime);
        }

        public override void Update() {
            if (timeUntilWalk > 0) {
                timeUntilWalk -= Time.deltaTime;
            } else {
                ai.TargetPos = targetPos;
                if (ai.MoveDir == Vector3.zero) {
                    ai.StateMachine.ChangeState(ai.ServeState);
                }
            }
        }
    }
}
