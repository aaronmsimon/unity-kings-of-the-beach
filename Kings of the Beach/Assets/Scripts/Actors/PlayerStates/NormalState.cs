using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.PlayerStates
{
    public class NormalState : PlayerBaseState
    {
        public NormalState(Player player) : base(player) { }

        public override void Enter() {
            Debug.Log("Entering the Player Normal state.");

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
                if (player.BallInfo.Possession == player.CourtSide) {
                    player.transform.position = player.Skills.ServingPartnerPos;
                } else {
                    player.transform.position = player.Skills.ReceivingPos;
                }
            }
        }

        private void OnBallHitGround() {
            player.StateMachine.ChangeState(player.PostPointState);
        }
    }
}
