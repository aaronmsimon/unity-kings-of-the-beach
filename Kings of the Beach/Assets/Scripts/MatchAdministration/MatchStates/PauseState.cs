using UnityEngine;
using KotB.Match;

namespace KotB.StatePattern.MatchStates
{
    public class PauseState : MatchBaseState
    {
        public PauseState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter() {
            matchManager.InputReader.EnableMenuInput();

            Time.timeScale = 0;
        }

        public override void Exit() {
            Time.timeScale = 1;
        }
    }
}
