using UnityEngine;
using KotB.Actors;
using KotB.StatePattern.MatchStates;

namespace KotB.StatePattern.PlayerStates
{
    public class NormalState : PlayerBaseState
    {
        public NormalState(Player player) : base(player) { }

        public override void Enter() {
            player.ServeCameraPriority.Value = 0;
            player.MainCameraPriority.Value = 10;
            player.UpdateCameraPriorty.Raise();

            player.BallHitGround += OnBallHitGround;
            player.MatchInfo.TransitionToServeState += OnMatchChangeToServeState;
        }

        public override void Exit() {
            player.BallHitGround -= OnBallHitGround;
            player.MatchInfo.TransitionToServeState -= OnMatchChangeToServeState;
        }

        public override void Update() {
            player.MoveDir = new Vector3(player.MoveInput.x, 0, player.MoveInput.y);

            if (player.MatchInfo.CurrentState is InPlayState && player.BallInfo.lastPlayerToHit != player) {
                float distanceToTarget = Vector3.Distance(player.transform.position, player.BallInfo.TargetPos);
                if (distanceToTarget <= player.Skills.TargetLockDistance) {
                    player.transform.position = player.BallInfo.TargetPos;
                    player.MoveDir = Vector3.zero;
                    player.StateMachine.ChangeState(player.LockState);
                }
            }            
        }

        private void OnMatchChangeToServeState() {
            if (player.MatchInfo.Server == player) {
                player.StateMachine.ChangeState(player.ServeState);
            } else {
                Vector3 newPos = player.MatchInfo.Server.CourtSide == player.CourtSide ? player.Skills.ServingPartnerPos : player.Skills.ReceivingPos;
                player.transform.position = new Vector3(newPos.x * player.CourtSide, newPos.y, newPos.z);
            }
        }

        private void OnBallHitGround() {
            player.MoveDir = Vector3.zero;
            player.StateMachine.ChangeState(player.PostPointState);
        }
    }
}
