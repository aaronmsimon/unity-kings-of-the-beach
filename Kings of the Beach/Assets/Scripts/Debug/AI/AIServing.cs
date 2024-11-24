using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class AIServing : MonoBehaviour
    {
        [SerializeField] private AI ai;
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
            ai.StateMachine.ChangeState(ai.ServeState);
            ai.BallInfo.GiveBall(ai);
        }
    }
}
