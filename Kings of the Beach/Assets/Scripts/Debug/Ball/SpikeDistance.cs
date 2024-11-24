using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class SpikeDistance : MonoBehaviour
    {
        [SerializeField] private Ball ball;
        [SerializeField] private Transform spikePoint;
        [SerializeField] private Transform target;

        private Athlete[] athletes;

        private void Start() {
            athletes = FindObjectsOfType<Athlete>();
        }

        private void Update() {
            if (Input.GetKey(KeyCode.Tab)) {
                ball.StateMachine.ChangeState(ball.GroundState);
                ball.transform.position = spikePoint.position;
                ClearAthletesState();
            }
            if (Input.GetKey(KeyCode.Space)) {
                ball.StateMachine.ChangeState(ball.InFlightState);
                ball.BallInfo.SetSpikeTarget(target.position, 2, null);
            }
        }

        private void ClearAthletesState() {
            foreach (Athlete athlete in athletes) {
                athlete.StateMachine.ChangeState(null);
            }
        }

        private void OnDrawGizmos() {
            if (spikePoint != null && target != null) {
                Vector3 distance = target.position - spikePoint.position;
                if (Physics.Raycast(spikePoint.position, distance.normalized, distance.magnitude)) {
                    Gizmos.color = Color.red;
                } else {
                    Gizmos.color = Color.green;
                }
                Gizmos.DrawLine(spikePoint.position, target.position);
            }
        }
    }
}
