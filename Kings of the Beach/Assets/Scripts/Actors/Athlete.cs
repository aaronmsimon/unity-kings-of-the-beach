using System;
using UnityEngine;
using KotB.StatePattern;
using KotB.Match;

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
        protected StateMachine stateMachine;
        protected float courtSideLength = 8;
        protected Vector3 moveDir;
        protected bool isJumping;

        private float noMansLand = 0.5f;
        private float skillLevelMax = 10;
        private float jumpFrames = 7;
        private float spikeFrames = 9;
        private float spikeFallFrames = 8;
        private float blockFrames = 9;
        private float blockFallFrames = 8;
        private float animationFrameRate = 20;
        private float animationTime;

        // caching
        private float moveSpeed;
        private float jumpHeight;
        private float reachPct;
        private CapsuleCollider capCollider;
        private SphereCollider sphereCollider;
        private Animator animator;


        protected virtual void Awake() {
            stateMachine = new StateMachine();

            capCollider = GetComponent<CapsuleCollider>();
            sphereCollider = GetComponent<SphereCollider>();
            animator = GetComponentInChildren<Animator>();
        }

        protected virtual void Start() {
            if (skills != null) {
                moveSpeed = skills.MoveSpeed;
                jumpHeight = skills.JumpHeight;
                reachPct = skills.ReachPct;
            } else {
                Debug.LogAssertion($"No skills found for { this.name }");
            }
        }

        protected virtual void Update() {
            stateMachine.Update();
            
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
            if (canMove) {
                // Animate
                if (newPos != transform.position) {
                    animator.SetBool("isWalking", true);
                } else {
                    animator.SetBool("isWalking", false);
                }
                // Move
                if (MathF.Abs(newPos.x) > noMansLand) {
                    transform.position = newPos;
                } else {
                    transform.position += new Vector3(0, 0, moveDir.z) * moveSpeed * Time.deltaTime;
                }
            }

            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
            skills.Position = transform.position;
        }

        private void Jump() {
            animationTime += Time.deltaTime;

            // Spiking
            if (animator.GetBool("isSpike")) {
                if (animationTime >= jumpFrames / animationFrameRate && animationTime < (jumpFrames + spikeFrames) / animationFrameRate) {
                    sphereCollider.enabled = true;
                    capCollider.enabled = false;
                } else if (animationTime >= (jumpFrames + spikeFrames) / animationFrameRate && animationTime < (jumpFrames + spikeFrames + spikeFallFrames) / animationFrameRate) {
                    sphereCollider.enabled = false;
                } else if (animationTime >= (jumpFrames + spikeFrames + spikeFallFrames) / animationFrameRate) {
                    animator.SetBool("isSpike", false);
                    isJumping = false;
                    capCollider.enabled = true;
                }
            }

            // Blocking
            if (animator.GetBool("isBlock")) {
                if (animationTime >= jumpFrames / animationFrameRate && animationTime < (jumpFrames + blockFrames) / animationFrameRate) {
                    sphereCollider.enabled = true;
                    capCollider.enabled = false;
                } else if (animationTime >= (jumpFrames + blockFrames) / animationFrameRate && animationTime < (jumpFrames + blockFrames + blockFallFrames) / animationFrameRate) {
                    sphereCollider.enabled = false;
                } else if (animationTime >= (jumpFrames + blockFrames + blockFallFrames) / animationFrameRate) {
                    animator.SetBool("isBlock", false);
                    isJumping = false;
                    capCollider.enabled = true;
                }
            }
        }

        public void PerformJump() {
            isJumping = true;
            animationTime = 0;
            transform.forward = new Vector3(-courtSide, 0, 0);
            if (courtSide == Mathf.Sign(ballInfo.Position.x)) {
                animator.SetBool("isSpike", true);
            } else {
                animator.SetBool("isBlock", true);
            }
            animator.SetTrigger("jump");
        }

        public void Pass(Vector3 targetPos) {
            BallInfo.SetPassTarget(targetPos, 7, 1.75f, this);
        }

        public void SetCourtSide(int courtSide) {
            this.courtSide = courtSide;
        }

        public void SwitchCourtSide() {
            courtSide *= -1;
        }

        protected virtual void OnDrawGizmos() {}

        //---- EVENT LISTENERS ----

        public void OnBallHitGround() {
            BallHitGround?.Invoke();
        }

        //---- PROPERTIES ----

        public SkillsSO Skills { get { return skills; } }
        public float SkillLevelMax { get { return skillLevelMax; } }
        public BallSO BallInfo { get { return ballInfo; } }
        public MatchInfoSO MatchInfo { get { return matchInfo; } }
        public StateMachine StateMachine { get { return stateMachine; } }
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
