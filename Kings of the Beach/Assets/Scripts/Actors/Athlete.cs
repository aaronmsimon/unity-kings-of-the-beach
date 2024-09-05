using UnityEngine;

namespace KotB.Actors
{
    public abstract class Athlete : MonoBehaviour
    {
        [Header("Skills")]
        [SerializeField] private SkillsSO skills;

        [Header("Ball")]
        [SerializeField] protected BallSO ballSO;

        [Header("Settings")]
        [SerializeField] private LayerMask obstaclesLayer;

        private bool canBump;
        private Ball _ball;
        private bool canUnlock;
        private float unlockTimer;
        private float unlockDelay = 0.25f;

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
                    TryUnlock();
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
            if (ballSO.ballState == BallState.Bump && ballSO.lastPlayerToHit != this) {
                float distanceToTarget = Vector3.Distance(transform.position, ballSO.Target);
                if (distanceToTarget <= skills.TargetLockDistance) {
                    transform.position = ballSO.Target;
                    athleteState = AthleteState.Locked;
                    canUnlock = false;
                    return;
                }
            }

            bool canMove = !Physics.Raycast(transform.position + Vector3.up * 0.5f, moveDir, out RaycastHit hit, 0.5f, obstaclesLayer);
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, moveDir, Color.red);
            if (canMove) {
                transform.position += moveDir * skills.MoveSpeed * Time.deltaTime;
            }
        }

        private void Bump() {
            bumpTimer -= Time.deltaTime;
            if (canBump && bumpTimer > 0 && _ball != null) {
                _ball.Bump(bumpTarget, 12, 2);
                canUnlock = true;
                unlockTimer = unlockDelay;
                canBump = false;
                ballSO.HitsForTeam += 1;
                Debug.Log("Hits: " + ballSO.HitsForTeam);
                ballSO.lastPlayerToHit = this;
            }
        }

        private void TryUnlock() {
            if (canUnlock) {
                unlockTimer -= Time.deltaTime;
                if (unlockTimer <= 0) {
                    athleteState = AthleteState.Normal;
                }
            }
        }

        //---- EVENT LISTENERS ----
        public void OnBallHitGround() {
            athleteState = AthleteState.Normal;
        }
    }
}
