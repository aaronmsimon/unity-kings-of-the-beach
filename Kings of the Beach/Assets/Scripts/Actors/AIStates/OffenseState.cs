using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class OffenseState : AIBaseState
    {
        public OffenseState(AI ai) : base(ai) { }

        private float teammateBuffer = 2;
        private float edgeBuffer = 0.5f;

        public override void Enter() {
            SetOffensePos();
            
            ai.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ai.BallInfo.TargetSet -= OnTargetSet;
        }

        private void SetOffensePos() {
            float targetZPos = ai.BallInfo.TargetPos.z;
            float zPos = Mathf.Lerp(targetZPos + teammateBuffer * -Mathf.Sign(targetZPos), (ai.CourtSideLength / 2 - edgeBuffer) * -Mathf.Sign(targetZPos), Random.value);
            ai.TargetPos = new Vector3(ai.Skills.OffenseXPos * ai.CourtSide, 0.01f, zPos);
        }

        private void OnTargetSet() {
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide) {
                ai.StateMachine.ChangeState(ai.DigReadyState);
            } else {
                ai.StateMachine.ChangeState(ai.DefenseState);
            }
        }
    }
}
