using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.PlayerStates
{
    public class PauseState : PlayerBaseState
    {
        public PauseState(Player player) : base(player) { }

        public override void Enter() {
            Time.timeScale = 0;
            player.InputReader.EnableMenuInput();
            player.PauseGame.Raise();
        }

        public override void Exit() {
            Time.timeScale = 1;
            player.InputReader.EnableBetweenPointsInput();
            player.UnpauseGame.Raise();
        }
    }
}
