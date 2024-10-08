using System;
using UnityEngine;
using KotB.Match;
using Unity.VisualScripting;

namespace KotB.Actors
{
    public abstract class Athlete : MonoBehaviour
    {
        [Header("Skills")]
        [SerializeField] protected SkillsSO skills;

        [Header("Scriptable Objects")]
        [SerializeField] protected BallSO ballInfo;
        [SerializeField] protected MatchInfoSO matchInfo;

        [Header("Settings")]
        [SerializeField] protected int courtSide;
        [SerializeField] private LayerMask obstaclesLayer;

        public event Action BallHitGround;

        protected Ball ball;
        protected float courtSideLength = 8;
        protected Vector3 moveDir;
        protected bool isJumping;

        private float noMansLand = 0.5f;
        private float skillLevelMax = 10;
        private float jumpDuration = 0.25f;
        private float jumpTimer;
        private bool jumpAscending;
        private float jumpDescendingMultiplier = 1.5f;
        private Vector3 startJumpPos;
        private Vector3 endJumpPos;
        private float reachPct = 1.25f;
        private CapsuleCollider capCollider;

        // caching
        private float moveSpeed;
        private float jumpHeight;

        private void Awake() {
            capCollider = GetComponent<CapsuleCollider>();
        }

        protected virtual void Start() {
            if (skills != null) {
                moveSpeed = skills.MoveSpeed;
                jumpHeight = skills.JumpHeight;
            } else {
                Debug.LogAssertion($"No skills found for { this.name }");
            }
        }

        protected virtual void Update() {
            if (!isJumping) {
                Move();
            } else {
                Jump();
            }
        }

        protected virtual void OnTriggerEnter(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                this.ball = ball;
            }
        }

        protected virtual void OnTriggerExit(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                this.ball = null;
            }
        }

        private void Move() {
            bool canMove = !Physics.Raycast(transform.position + Vector3.up * 0.5f, moveDir, out RaycastHit hit, 0.5f, obstaclesLayer);
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, moveDir, Color.red);
            Vector3 newPos = transform.position + moveDir * moveSpeed * Time.deltaTime;
            if (canMove && MathF.Abs(newPos.x) > noMansLand) {
                transform.position = newPos;
            }

            skills.Position = transform.position;
        }

        private void Jump() {
            jumpTimer += Time.deltaTime;
            if (jumpAscending) {
                if (jumpTimer < jumpDuration) {
                    float t = jumpTimer / jumpDuration;
                    transform.position = Vector3.Lerp(startJumpPos, endJumpPos, t);
                } else {
                    transform.position = endJumpPos;
                    jumpAscending = false;
                    jumpTimer = 0;
                }
            } else {
                if (jumpTimer < jumpDuration / jumpDescendingMultiplier) {
                    float t = jumpTimer / jumpDuration;
                    transform.position = Vector3.Lerp(endJumpPos, startJumpPos, t);
                } else {
                    transform.position = startJumpPos;
                    isJumping = false;
                    capCollider.center *= 1 / reachPct;
                    capCollider.height *= 1 / reachPct;
                }
            }
        }

        public void PerformJump() {
            isJumping = true;
            capCollider.center *= reachPct;
            capCollider.height *= reachPct;
            jumpTimer = 0;
            jumpAscending = true;
            startJumpPos = transform.position;
            endJumpPos = new Vector3(transform.position.x, jumpHeight, transform.position.z);
        }

        public void SetCourtSide(int courtSide) {
            this.courtSide = courtSide;
        }

        public void SwitchCourtSide() {
            courtSide *= -1;
        }

        protected virtual void OnDrawGizmos() {
            Debug.DrawLine(transform.position, ballInfo.Position, Color.green);
        }

        //---- EVENT LISTENERS ----

        public void OnBallHitGround() {
            BallHitGround?.Invoke();
        }

        //---- PROPERTIES ----

        public SkillsSO Skills { get { return skills; } }
        public float SkillLevelMax { get { return skillLevelMax; } }
        public BallSO BallInfo { get { return ballInfo; } }
        public MatchInfoSO MatchInfo { get { return matchInfo; } }
        public int CourtSide { get { return courtSide; } }
        public float CourtSideLength { get { return courtSideLength; } }
        public Vector3 MoveDir {
            get { return moveDir; }
            set { moveDir = value; }
        }
        public Ball Ball { get { return ball; } }
        public bool IsJumping { get { return isJumping; } }
    }
}
