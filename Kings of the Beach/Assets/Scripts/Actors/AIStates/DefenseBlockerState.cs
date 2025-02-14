using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class DefenseBlockerState : AIBaseState
    {
        public DefenseBlockerState(AI ai) : base(ai) { }
        
        private Vector3 targetPos;

        private float blockPos = 1;

        public override void Enter() {
            ai.TargetPos = ai.transform.position;
            
            ai.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ai.BallInfo.TargetSet -= OnTargetSet;
        }

        public override void Update() {
            ai.TargetPos = targetPos;
        }

        public override void OnTriggerEnter(Collider other) {
            if (ai.Ball != null) {
                ai.BlockAttempt();
            }
        }

        private void OnTargetSet() {
            // Need to add consideration if a shot is not blockable
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide) {
                ai.PerformJump();
                ai.StateMachine.ChangeState(ai.OffenseState);
            } else {
                targetPos = new Vector3(blockPos * ai.CourtSide, ai.transform.position.y, ai.BallInfo.TargetPos.z);
            }
        }
    }
}
