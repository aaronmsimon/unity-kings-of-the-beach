using UnityEngine;

namespace KotB.Testing
{
    public class SpikeDistance : MonoBehaviour
    {
        [SerializeField] private Ball ball;
        [SerializeField] private Transform spikePoint;
        [SerializeField] private Transform target;

        private void Update() {
            if (Input.GetKey(KeyCode.Tab)) {
                ball.StateMachine.ChangeState(ball.GroundState);
                ball.transform.position = spikePoint.position;
            }
            if (Input.GetKey(KeyCode.Space)) {
                ball.StateMachine.ChangeState(ball.InFlightState);
                ball.BallInfo.SetSpikeTarget(target.position, 2, null);
            }
        }

        private void OnDrawGizmos() {
            if (spikePoint != null && target != null) {
                if (Physics.Raycast(spikePoint.position, target.position, out RaycastHit hit)) {
                    Gizmos.color = Color.red;
                } else {
                    Gizmos.color = Color.green;
                }
                Gizmos.DrawLine(spikePoint.position, target.position);
            }
        }
    }
}
