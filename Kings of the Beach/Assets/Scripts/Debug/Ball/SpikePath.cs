using UnityEngine;
using RoboRyanTron.Unite2017.Events;
using KotB.Items;

namespace KotB.Testing
{
    public class SpikePath : MonoBehaviour
    {
        [SerializeField] private Transform startPos;
        [SerializeField] private Transform target;
        [SerializeField] private InputReader inputReader;
        [SerializeField] private float spikeTime = 1.5f;
        [SerializeField] private GameEvent ResetBallEvent;

        private Ball ball;

        private void Awake() {
            ball = GetComponent<Ball>();
        }

        private void Start() {
            ResetBall();
        }

        private void OnEnable() {
            inputReader.testEvent += SpikeBall;
        }

        private void OnDisable() {
            inputReader.testEvent -= SpikeBall;
        }

        private void Update() {
            if (Input.GetKey(KeyCode.Space)) {
                ResetBall();
            }
        }

        private void ResetBall() {
            ResetBallEvent.Raise();
            ball.transform.position = startPos.position;
        }

        private void SpikeBall() {
            ball.StateMachine.ChangeState(ball.InFlightState);
            ball.BallInfo.SetSpikeTarget(target.position, spikeTime, null, Stats.StatTypes.Attack);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(startPos.position, target.position);
        }
    }
}
