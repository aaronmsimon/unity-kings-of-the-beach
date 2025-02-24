using UnityEngine;
using KotB.Match;
using System.Collections.Generic;

namespace KotB.StatePattern.MatchStates
{
    public class PostPointState : MatchBaseState
    {
        public PostPointState(MatchManager matchManager) : base(matchManager) { }

        private float timeUntilChangeState;
        private float waitTime = 3f;

        public override void Enter() {
            // matchManager.InputReader.EnableBetweenPointsInput();
            // matchManager.InputReader.interactEvent += OnInteract;

            timeUntilChangeState = waitTime;

            CheckGameEnd();
        }

        public override void Exit() {
            // matchManager.InputReader.interactEvent -= OnInteract;
        }

        public override void Update() {
            timeUntilChangeState -= Time.deltaTime;

            if (timeUntilChangeState < 0) {
                matchManager.StateMachine.ChangeState(matchManager.PrePointState);
            }
        }

        private void OnInteract() {
            // matchManager.MatchInfo.TransitionToServeStateEvent();
            // matchManager.StateMachine.ChangeState(matchManager.ServeState);
        }

        private void CheckGameEnd() {
            List<TeamSO> teams = matchManager.MatchInfo.Teams;
            for (int i = 0; i < teams.Count; i++) {
                if (teams[i].Score >= matchManager.MatchInfo.ScoreToWin && Mathf.Abs(teams[i].Score - teams[1 - i].Score) > 1 && teams[i].Score > teams[1 - i].Score) {
                    Debug.Log($"{teams[i].TeamName.Value} wins!");
                    matchManager.StateMachine.ChangeState(matchManager.MatchEndState);
                }
            }
        }
    }
}
