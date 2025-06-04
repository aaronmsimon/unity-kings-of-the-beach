using UnityEngine;
using KotB.Match;
using KotB.Stats;

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
            // Determine Scoring Team
            TeamSO scoringTeam;
            if (matchManager.BallInfo.IsInBounds(matchManager.BallInfo.Position) && Mathf.Sign(matchManager.BallInfo.Position.x) == -matchManager.BallInfo.LastPlayerToHit.CourtSide) {
                scoringTeam = matchManager.MatchInfo.GetTeam(matchManager.BallInfo.LastPlayerToHit);
            } else {
                scoringTeam = matchManager.MatchInfo.GetOpposingTeam(matchManager.BallInfo.LastPlayerToHit);
            }

            // Add Score
            scoringTeam?.AddScore(1);

            // Update Stats
            StatTypes statType = matchManager.BallInfo.LastStatType;
            switch (statType) {
                case StatTypes.Serve:
                    if (scoringTeam == matchManager.MatchInfo.GetTeam(matchManager.BallInfo.LastPlayerToHit)) {
                        statType = StatTypes.ServiceAce;
                    } else {
                        statType = StatTypes.ServiceError;
                    }
                    break;
                case StatTypes.Block:
                    if (scoringTeam == matchManager.MatchInfo.GetTeam(matchManager.BallInfo.LastPlayerToHit)) {
                        statType = StatTypes.BlockPoint;
                    } else {
                        statType = StatTypes.BlockError;
                    }
                    break;
                case StatTypes.Attack:
                    if (scoringTeam == matchManager.MatchInfo.GetTeam(matchManager.BallInfo.LastPlayerToHit)) {
                        statType = StatTypes.AttackKill;
                    } else {
                        statType = StatTypes.AttackError;
                    }
                    break;
            }
            matchManager.BallInfo.StatUpdate.Raise(matchManager.BallInfo.LastPlayerToHit, statType);

            // Sideout, if needed
            if (scoringTeam != matchManager.MatchInfo.Teams[matchManager.MatchInfo.TeamServeIndex]) {
                matchManager.MatchInfo.SideOut();
            }

            // Update Score & Change State
            matchManager.ScoreUpdate();

            matchManager.InvokePointComplete();
        }
    }
}
