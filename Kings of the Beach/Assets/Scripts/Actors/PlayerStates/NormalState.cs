using UnityEngine;
using KotB.Actors;
using KotB.StatePattern.MatchStates;
using KotB.Items;

namespace KotB.StatePattern.PlayerStates
{
    public class NormalState : PlayerBaseState
    {
        public NormalState(Player player) : base(player) { }

        public override void Enter() {
            player.ServeCameraPriority.Value = 0;
            player.MainCameraPriority.Value = 10;
            player.UpdateCameraPriorty.Raise();

            player.BlockTrigger.Triggered += OnBlockTriggered;
            player.MatchInfo.TransitionToServeState += OnMatchChangeToServeState;
        }

        public override void Exit() {
            player.BlockTrigger.Triggered -= OnBlockTriggered;
            player.MatchInfo.TransitionToServeState -= OnMatchChangeToServeState;
        }

        public override void Update() {
            player.MoveDir = new Vector3(player.MoveInput.x, 0, player.MoveInput.y);

            if (player.MatchInfo.CurrentState is InPlayState && player.BallInfo.LastPlayerToHit != player && Mathf.Sign(player.BallInfo.TargetPos.x) == player.CourtSide && !player.BallInfo.LockedOn) {
                float distanceToTarget = (player.transform.position - player.BallInfo.TargetPos).sqrMagnitude;
                if (distanceToTarget <= player.Skills.TargetLockDistance * player.Skills.TargetLockDistance && Mathf.Abs(player.BallInfo.TargetPos.x) > player.NoMansLand) {
                    Vector2 lockTowardsTarget = player.LockTowardsTarget();
                    Vector3 lockPos = new Vector3(lockTowardsTarget.x, 0.01f, lockTowardsTarget.y);
                    player.transform.position = lockPos;
                    player.MoveDir = Vector3.zero;
                    player.BallInfo.LockedOn = true;
                    player.StateMachine.ChangeState(player.LockState);
                }
            }            
        }

        private void OnBlockTriggered(Collider other) {
            player.BlockAttempt(other);
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
