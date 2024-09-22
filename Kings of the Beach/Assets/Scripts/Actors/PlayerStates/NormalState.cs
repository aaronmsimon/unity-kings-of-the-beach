using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.PlayerStates
{
    public class NormalState : PlayerBaseState
    {
        public NormalState(Player player) : base(player) { }

        public override void Enter() {
            Debug.Log("Entering the Player Normal state.");

            player.ServeCameraPriority.Value = 0;
            player.MainCameraPriority.Value = 10;
            player.UpdateCameraPriorty.Raise();

            player.BallHitGround += OnBallHitGround;
            player.MatchChangeToServeState += OnMatchChangeToServeState;
        }

        public override void Exit() {
            player.BallHitGround -= OnBallHitGround;
            player.MatchChangeToServeState -= OnMatchChangeToServeState;

            Debug.Log("Exiting the Player Normal State.");
        }

        public override void Update()
        {
            player.MoveDir = new Vector3(player.MoveInput.x, 0, player.MoveInput.y);

            if (player.BallInfo.lastPlayerToHit != player) {
                float distanceToTarget = Vector3.Distance(player.transform.position, player.BallInfo.Target);
                if (distanceToTarget <= player.Skills.TargetLockDistance) {
                    player.transform.position = player.BallInfo.Target;
                    player.StateMachine.ChangeState(player.LockState);
                }
            }            
        }

        private void OnMatchChangeToServeState() {
            if (player.MatchInfo.Server == player) {
                player.StateMachine.ChangeState(player.ServeState);
            } else {
                Vector3 newPos;
                if (player.BallInfo.Possession == player.CourtSide) {
                    newPos = player.Skills.ServingPartnerPos;
                } else {
                    newPos = player.Skills.ReceivingPos;
                }
                player.transform.position = new Vector3(newPos.x * player.CourtSide, newPos.y, newPos.z);
            }
        }

        private void OnBallHitGround() {
            player.StateMachine.ChangeState(player.PostPointState);
        }
    }
}
