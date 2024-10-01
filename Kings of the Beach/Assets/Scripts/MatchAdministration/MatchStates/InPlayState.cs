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
            int lastPlayerTeamIndex = matchManager.GetTeamIndex(matchManager.BallInfo.lastPlayerToHit);

            if (matchManager.BallInfo.IsInBounds() && Mathf.Sign(matchManager.BallInfo.Position.x) == -matchManager.BallInfo.lastPlayerToHit.CourtSide) {
                matchManager.Teams[lastPlayerTeamIndex]?.AddScore(1);
                if (lastPlayerTeamIndex != matchManager.MatchInfo.TeamServeIndex) NextServer();
            } else {
                matchManager.Teams[matchManager.GetOpponentIndex(lastPlayerTeamIndex)]?.AddScore(1);
                if (lastPlayerTeamIndex == matchManager.MatchInfo.TeamServeIndex) NextServer();
            }


            matchManager.MatchInfo.TotalPoints += 1;
            matchManager.ScoreChanged.Raise();

            CheckGameEnd();
        }

        private void CheckGameEnd() {
            for (int i = 0; i < matchManager.MatchInfo.Teams.Length; i++) {
                if (matchManager.MatchInfo.Teams[i].Score == matchManager.MatchInfo.ScoreToWin) {
                    Debug.Log($"{matchManager.MatchInfo.Teams[i].TeamName} wins!");
                    matchManager.StateMachine.ChangeState(matchManager.MatchEndState);
                }
            }
            matchManager.StateMachine.ChangeState(matchManager.PostPointState);
        }

        private void NextServer() {
            matchManager.MatchInfo.TeamServeIndex = Mathf.Abs(matchManager.MatchInfo.TeamServeIndex - 1);
            if (matchManager.MatchInfo.TeamServeIndex == 0)
                matchManager.MatchInfo.PlayerServeIndex = Mathf.Abs(matchManager.MatchInfo.PlayerServeIndex - 1);
        }
    }
}
