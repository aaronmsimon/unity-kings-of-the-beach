using UnityEngine;
using KotB.Match;

namespace KotB.StatePattern.MatchStates
{
    public class PauseState : MatchBaseState
    {
        public PauseState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter() {
            matchManager.InputReader.EnableMenuInput();

            matchManager.InputReader.pauseEvent += OnResume;

            Time.timeScale = 0;
        }

        public override void Exit() {
            matchManager.InputReader.pauseEvent += OnResume;

            Time.timeScale = 1;
        }

        private void OnResume() {
            matchManager.StateMachine.ChangeState(matchManager.StateBeforePause);
        }
    }
}
