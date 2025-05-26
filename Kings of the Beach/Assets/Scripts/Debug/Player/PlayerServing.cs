using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class PlayerServing : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private BallPath ballPath;

        private void Start() {
            GoToServeState();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                GoToServeState();
                ballPath.DestroyAllPathMarkers();
            }
        }

        private void GoToServeState() {
            // player.StateMachine.ChangeState(player.ServeState);
            player.BallInfo.GiveBall(player);
        }
    }
}
