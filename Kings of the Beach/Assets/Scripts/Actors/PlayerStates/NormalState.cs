using UnityEngine;
using KotB.Actors;
using KotB.StatePattern.MatchStates;

namespace KotB.StatePattern.PlayerStates
{
    public class NormalState : PlayerBaseState
    {
        public NormalState(Player player) : base(player) { }

        private bool blockAttempted;

        public override void Enter() {
            player.ServeCameraPriority.Value = 0;
            player.MainCameraPriority.Value = 10;
            player.UpdateCameraPriorty.Raise();

            blockAttempted = false;

            player.MatchInfo.TransitionToServeState += OnMatchChangeToServeState;
        }

        public override void Exit() {
            player.MatchInfo.TransitionToServeState -= OnMatchChangeToServeState;
        }

        public override void Update() {
            player.MoveDir = new Vector3(player.MoveInput.x, 0, player.MoveInput.y);

            if (player.MatchInfo.CurrentState is InPlayState && player.BallInfo.LastPlayerToHit != player && Mathf.Sign(player.BallInfo.TargetPos.x) == player.CourtSide && !player.BallInfo.LockedOn) {
                float distanceToTarget = (player.transform.position - player.BallInfo.TargetPos).sqrMagnitude;
                if (distanceToTarget <= player.Skills.TargetLockDistance * player.Skills.TargetLockDistance && Mathf.Abs(player.BallInfo.TargetPos.x) > player.NoMansLand) {
                    player.transform.position = player.BallInfo.TargetPos;
                    player.MoveDir = Vector3.zero;
                    player.BallInfo.LockedOn = true;
                    player.StateMachine.ChangeState(player.LockState);
                }
            }            
        }

        public override void OnTriggerEnter(Collider other) {
            if (player.Ball != null) {
                if (player.SpikeBlockCollider.enabled & !blockAttempted) {
                    player.BlockAttempt();
                    blockAttempted = true;
                }
            }
        }

        private void OnMatchChangeToServeState() {
            if (player.MatchInfo.GetServer() == player) {
                player.StateMachine.ChangeState(player.ServeState);
            } else {
                player.transform.position = player.ServeDefPos;
                player.FaceOpponent();
            }
        }
    }
}
