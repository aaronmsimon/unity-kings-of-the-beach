using UnityEngine;

namespace KotB.Actors
{
    public class Athlete : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private LayerMask targetLayer;

        [SerializeField] private InputReader inputReader;

        private Vector2 moveInput;
        private AthleteState athleteState;

        private enum AthleteState {
            Normal,
            Locked
        }

        private void Start() {
            athleteState = AthleteState.Normal;
        }

        //Adds listeners for events being triggered in the InputReader script
        private void OnEnable()
        {
            inputReader.moveEvent += OnMove;
        }
        
        //Removes all listeners to the events coming from the InputReader script
        private void OnDisable()
        {
            inputReader.moveEvent -= OnMove;
        }

        private void Update() {
            switch (athleteState) {
                case AthleteState.Normal:
                    Move();
                    CheckForTarget();
                    break;
                case AthleteState.Locked:
                    break;
                default:
                    Debug.LogWarning("Athlete State unhandled.");
                    break;
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                ball.StopAllCoroutines();
                ball.Bump(new Vector3(5,0,2), 12, 2);
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

        //---- EVENT LISTENERS ----

        private void OnMove(Vector2 movement)
        {
            moveInput = movement;
        }

        public void OnTargetDestroyed() {
            athleteState = AthleteState.Normal;
        }
    }
}
