using UnityEngine;

namespace KotB.Actors
{
    public abstract class Athlete : MonoBehaviour
    {
        [Header("Skills")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float targetLockDistance;

        [Header("Ball")]
        [SerializeField] protected BallSO ballSO;

        [Header("Settings")]
        [SerializeField] private LayerMask obstaclesLayer;

        private bool canBump;
        private Ball _ball;

        protected Vector3 moveDir;
        protected AthleteState athleteState;
        protected float bumpTimer;
        protected Vector3 bumpTarget;

        protected enum AthleteState {
            Normal,
            Locked
        }

        private void Start() {
            athleteState = AthleteState.Normal;
            canBump = false;
            bumpTimer = 0;
        }

        protected virtual void Update() {
            switch (athleteState) {
                case AthleteState.Normal:
                    Move();
                    break;
                case AthleteState.Locked:
                    Bump();
                    break;
                default:
                    Debug.LogWarning("Athlete State unhandled.");
                    break;
            }
        }

        protected virtual void OnTriggerEnter(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                _ball = ball;
                canBump = true;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                canBump = false;
            }
        }

        private void Move() {
            if (ballSO.ballState == BallState.Bump) {
                float distanceToTarget = Vector3.Distance(transform.position, ballSO.Target);
                if (distanceToTarget <= targetLockDistance) {
                    transform.position = ballSO.Target;
                    athleteState = AthleteState.Locked;
                    return;
                }
            }

            bool canMove = !Physics.Raycast(transform.position + Vector3.up * 0.5f, moveDir, out RaycastHit hit, 0.5f, obstaclesLayer);
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, moveDir, Color.red);
            if (canMove) {
                transform.position += moveDir * moveSpeed * Time.deltaTime;
            }
        }

        private void Bump() {
            bumpTimer -= Time.deltaTime;
            if (canBump && bumpTimer > 0 && _ball != null)
                _ball.Bump(bumpTarget, 12, 2);
        }
    }
}
