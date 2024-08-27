using UnityEngine;

namespace KotB.Actors
{
    public class Athlete : MonoBehaviour
    {
        [Header("Player Controls")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float coyoteTime;

        [Header("Settings")]
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private InputReader inputReader;

        private Vector2 moveInput;
        private AthleteState athleteState;
        private bool canBump;
        private Ball _ball;
        private float bumpTimer;
        private Vector3 bumpTarget;

        private enum AthleteState {
            Normal,
            Locked
        }

        private void Start() {
            athleteState = AthleteState.Normal;
            canBump = false;
            bumpTimer = 0;
        }

        //Adds listeners for events being triggered in the InputReader script
        private void OnEnable()
        {
            inputReader.moveEvent += OnMove;
            inputReader.bumpEvent += OnBump;
        }
        
        //Removes all listeners to the events coming from the InputReader script
        private void OnDisable()
        {
            inputReader.moveEvent -= OnMove;
            inputReader.bumpEvent -= OnBump;
        }

        private void Update() {
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
        
        private Vector2 CircleMappedToSquare(float u, float v) {
            float u2 = u * u;
            float v2 = v * v;
            float tworoot2 = 2 * Mathf.Sqrt(2);
            float subtermX = 2 + u2 - v2;
            float subtermY = 2 - u2 + v2;
            float termX1 = subtermX + u * tworoot2;
            float termX2 = subtermX - u * tworoot2;
            float termY1 = subtermY + v * tworoot2;
            float termY2 = subtermY - v * tworoot2;

            float epsilon = 0.0000001f;
            if (termX1 < epsilon)
                termX1 = 0;
            if (termX2 < epsilon)
                termX2 = 0;
            if (termY1 < epsilon)
                termY1 = 0;
            if (termY2 < epsilon)
                termY2 = 0;

            float x = Mathf.Clamp(0.5f * Mathf.Sqrt(termX1) - 0.5f * Mathf.Sqrt(termX2), -1, 1);
            float y = Mathf.Clamp(0.5f * Mathf.Sqrt(termY1) - 0.5f * Mathf.Sqrt(termY2), -1, 1);

            return new Vector2(x, y);
        }

        //---- EVENT LISTENERS ----

        private void OnMove(Vector2 movement)
        {
            moveInput = movement;
        }

        private void OnBump() {
            bumpTimer = coyoteTime;

            Vector2 aim = CircleMappedToSquare(moveInput.x, moveInput.y);

            float targetX = aim.x * 5 + 4;
            float targetZ = aim.y * 5;
            bumpTarget = new Vector3(targetX, 0f, targetZ);
        }

        public void OnTargetDestroyed() {
            athleteState = AthleteState.Normal;
        }
    }
}
