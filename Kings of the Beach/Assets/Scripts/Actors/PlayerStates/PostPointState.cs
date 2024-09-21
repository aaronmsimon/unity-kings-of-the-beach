using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.PlayerStates
{
    public class PostPointState : PlayerBaseState
    {
        public PostPointState(Player player) : base(player) { }

        public override void Enter() {
            Debug.Log("Entering the Player Post Point state.");
        }

        public override void Exit() {
            Debug.Log("Exiting the Player Post Point State.");
        }
    }
}
