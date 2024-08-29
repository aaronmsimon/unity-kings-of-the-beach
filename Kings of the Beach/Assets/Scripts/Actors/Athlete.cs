using UnityEngine;

namespace KotB.Actors
{
    public class Athlete : MonoBehaviour
    {
        [Header("Athlete Settings")]
        [SerializeField] protected float moveSpeed;

        [Header("Ball")]
        [SerializeField] protected BallSO ballSO;

        [Header("Settings")]
        [SerializeField] private LayerMask targetLayer;

        private bool canBump;
        private Ball _ball;

        protected Vector2 moveInput;
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
                    CheckForTarget();
                    break;
                case AthleteState.Locked:
                    Bump();
                    break;
                default:
                    Debug.LogWarning("Athlete State unhandled.");
                    break;
            }
        }

        private void OnTriggerEnter(Collider other) {
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
            Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);

            bool canMove = !Physics.Raycast(transform.position + Vector3.up * 0.5f, moveDir, out RaycastHit hit, 0.5f);
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, moveDir, Color.red);
            if (canMove) {
                transform.position += moveDir * moveSpeed * Time.deltaTime;
            }
        }

        private void CheckForTarget() {
            bool isOverTarget = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hitInfo, 1f, targetLayer);

            if (isOverTarget) {
                transform.position = hitInfo.transform.position;
                athleteState = AthleteState.Locked;
            }
        }

        private void Bump() {
            bumpTimer -= Time.deltaTime;
            if (canBump && bumpTimer > 0 && _ball != null)
                _ball.Bump(bumpTarget, 12, 2);
        }
    }
}
