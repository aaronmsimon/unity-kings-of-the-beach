using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class SetPlayerState : MonoBehaviour
    {
        private Player player;

        private void Awake() {
            player = GetComponent<Player>();
        }

        public void SetPlayerState_Normal() {
            player.StateMachine.ChangeState(player.NormalState);
        }
    }
}
