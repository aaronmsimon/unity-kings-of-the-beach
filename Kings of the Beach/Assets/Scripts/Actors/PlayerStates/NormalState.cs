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

            player.transform.forward = Vector3.right * -player.CourtSide;

            player.MatchInfo.TransitionToServeState += OnMatchChangeToServeState;
        }

        public override void Exit() {
            player.MatchInfo.TransitionToServeState -= OnMatchChangeToServeState;
        }

        public override void Update() {
            player.MoveDir = new Vector3(player.MoveInput.x, 0, player.MoveInput.y);

            if (player.MatchInfo.CurrentState is InPlayState && player.BallInfo.lastPlayerToHit != player && Mathf.Sign(player.BallInfo.TargetPos.x) == player.CourtSide && !player.BallInfo.LockedOn) {
                float distanceToTarget = (player.transform.position - player.BallInfo.TargetPos).sqrMagnitude;
                if (distanceToTarget <= player.Skills.TargetLockDistance * player.Skills.TargetLockDistance) {
                    player.transform.position = player.BallInfo.TargetPos;
                    player.MoveDir = Vector3.zero;
                    player.BallInfo.LockedOn = true;
                    player.StateMachine.ChangeState(player.LockState);
                }
            }            
        }

        public override void OnTriggerEnter(Collider other) {
            if (player.Ball != null) {
                player.BlockAttempt();
            }
        }

        private void OnMatchChangeToServeState() {
            if (player.MatchInfo.GetServer() == player) {
                player.StateMachine.ChangeState(player.ServeState);
            } else {
                Athlete test = player.MatchInfo.GetServer();
                // Vector3 newPos = player.MatchInfo.GetServer().CourtSide == player.CourtSide ? player.Skills.ServingPartnerPos : new Vector3(player.Skills.DefensePos.x, 0.01f, -2);
                // player.transform.position = new Vector3(newPos.x * player.CourtSide, newPos.y, newPos.z);
                player.transform.position = player.ServeDefPos;
                player.transform.forward = Vector3.right * -player.CourtSide;
            }
        }
    }
}
