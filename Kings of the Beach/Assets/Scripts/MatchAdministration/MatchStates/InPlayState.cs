using UnityEngine;
using KotB.Match;

namespace KotB.StatePattern.MatchStates
{
    public class InPlayState : MatchBaseState
    {
        public InPlayState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter() {
            matchManager.InputReader.EnableGameplayInput();

            matchManager.BallHitGround += OnBallHitGround;
        }

        public override void Exit()
        {
            matchManager.BallHitGround -= OnBallHitGround;
        }

        private void OnBallHitGround() {
            Team scoringTeam;
            if (matchManager.BallInfo.IsInBounds(matchManager.BallInfo.Position) && Mathf.Sign(matchManager.BallInfo.Position.x) == -matchManager.BallInfo.lastPlayerToHit.CourtSide) {
                scoringTeam = matchManager.MatchInfo.GetTeam(matchManager.BallInfo.lastPlayerToHit);
            } else {
                scoringTeam = matchManager.MatchInfo.GetOpposingTeam(matchManager.BallInfo.lastPlayerToHit);
            }
            scoringTeam?.AddScore(1);
            if (!scoringTeam.Serving) Sideout();

            matchManager.ScoreUpdate();            
        }

        private void Sideout() {
            foreach (Team team in matchManager.MatchInfo.Teams) {
                team.Serving = !team.Serving;
            }
        }
    }
}
