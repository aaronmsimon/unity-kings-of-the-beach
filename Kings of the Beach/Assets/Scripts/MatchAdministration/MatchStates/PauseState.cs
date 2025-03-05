using UnityEngine;
using KotB.Match;

namespace KotB.StatePattern.MatchStates
{
    public class PauseState : MatchBaseState
    {
        public PauseState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter() {
            matchManager.InputReader.EnableMenuInput();
            
            matchManager.MatchInfo.TogglePause += OnPause;

            Time.timeScale = 0;
        }

        public override void Exit() {
            matchManager.MatchInfo.TogglePause -= OnPause;

            Time.timeScale = 1;
        }

        private void OnPause(bool pause) {
            matchManager.StateMachine.ChangeState(matchManager.StateBeforePause);
        }
    }
}
