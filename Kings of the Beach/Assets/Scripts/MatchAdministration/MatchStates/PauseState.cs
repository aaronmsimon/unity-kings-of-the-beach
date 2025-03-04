using UnityEngine;
using KotB.Match;

namespace KotB.StatePattern.MatchStates
{
    public class PauseState : MatchBaseState
    {
        public PauseState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter() {
            matchManager.InputReader.EnableMenuInput();
            
            matchManager.InputReader.pauseEvent += OnPause;

            Time.timeScale = 0;
            Debug.Log("game paused");
        }

        public override void Exit() {
            matchManager.InputReader.pauseEvent -= OnPause;

            Time.timeScale = 1;
            matchManager.Paused = false;
            Debug.Log("game unpaused");
        }

        private void OnPause() {
            matchManager.StateMachine.ChangeState(matchManager.StateBeforePause);
        }
    }
}
