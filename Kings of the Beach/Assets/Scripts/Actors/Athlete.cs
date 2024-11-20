using System;
using UnityEngine;
using KotB.StatePattern;
using KotB.Match;
using System.Collections.Generic;

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
        [SerializeField] private LayerMask invalidAimLayer;

        protected Ball ball;
        protected StateMachine stateMachine;
        protected float courtSideLength = 8;
        protected Vector3 moveDir;
        protected bool isJumping;

        private float defaultHeight = 1.9f;
        private float noMansLand = 0.5f;
        private float skillLevelMax = 10;
        private float jumpFrames = 7;
        private float spikeFrames = 9;
        private float spikeFallFrames = 8;
        private float blockFrames = 9;
        private float blockFallFrames = 8;
        protected float animationFrameRate = 24;
        private float jumpAimationTime;
        private float reachHeight;
        private Athlete teammate;
        private List<Athlete> opponents;

        // caching
        private float moveSpeed;
        private CapsuleCollider capCollider;
        private SphereCollider sphereCollider;
        protected Animator animator;
        private Transform leftHandEnd;


        protected virtual void Awake() {
            stateMachine = new StateMachine();

            capCollider = GetComponent<CapsuleCollider>();
            sphereCollider = GetComponent<SphereCollider>();
            animator = GetComponentInChildren<Animator>();
        }

        protected virtual void Start() {
            if (skills != null) {
                moveSpeed = skills.MoveSpeed;
                float percentScale = skills.Height / defaultHeight;

                transform.localScale = new Vector3(percentScale, percentScale, percentScale);
                reachHeight = (sphereCollider.center.y + sphereCollider.radius) * percentScale;
            } else {
                Debug.LogAssertion($"No skills found for { this.name }");
            }

            leftHandEnd = transform.Find("Volleyball-Character").Find("CCM-Armature").Find("Pelvis").Find("Spine1").Find("Spine2").Find("Shoulder.L").Find("UpperArm.L").Find("LowerArm.L").Find("Hand.L").Find("Hand.L_end");
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

            stateMachine.OnTriggerEnter(other);
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
            if (moveDir != Vector3.zero) transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
            skills.Position = transform.position;
        }

        private void Jump() {
            jumpAimationTime += Time.deltaTime;

            // Spiking
            if (animator.GetBool("isSpike")) {
                if (jumpAimationTime >= jumpFrames / animationFrameRate && jumpAimationTime < (jumpFrames + spikeFrames) / animationFrameRate) {
                    sphereCollider.enabled = true;
                    capCollider.enabled = false;
                } else if (jumpAimationTime >= (jumpFrames + spikeFrames) / animationFrameRate && jumpAimationTime < (jumpFrames + spikeFrames + spikeFallFrames) / animationFrameRate) {
                    sphereCollider.enabled = false;
                } else if (jumpAimationTime >= (jumpFrames + spikeFrames + spikeFallFrames) / animationFrameRate) {
                    animator.SetBool("isSpike", false);
                    isJumping = false;
                    capCollider.enabled = true;
                }
            }

            // Blocking
            if (animator.GetBool("isBlock")) {
                if (jumpAimationTime >= jumpFrames / animationFrameRate && jumpAimationTime < (jumpFrames + blockFrames) / animationFrameRate) {
                    sphereCollider.enabled = true;
                    capCollider.enabled = false;
                } else if (jumpAimationTime >= (jumpFrames + blockFrames) / animationFrameRate && jumpAimationTime < (jumpFrames + blockFrames + blockFallFrames) / animationFrameRate) {
                    sphereCollider.enabled = false;
                } else if (jumpAimationTime >= (jumpFrames + blockFrames + blockFallFrames) / animationFrameRate) {
                    animator.SetBool("isBlock", false);
                    isJumping = false;
                    capCollider.enabled = true;
                }
            }
        }

        public void PerformJump() {
            isJumping = true;
            jumpAimationTime = 0;
            transform.forward = new Vector3(-courtSide, 0, 0);
            if (courtSide == Mathf.Sign(ballInfo.Position.x)) {
                animator.SetBool("isSpike", true);
            } else {
                animator.SetBool("isBlock", true);
            }
            animator.SetTrigger("jump");
        }

        public void Pass(Vector3 targetPos, float height, float time) {
            ballInfo.SetPassTarget(targetPos, height, time, this);
        }

        public void Spike(Vector3 targetPos) {
            // Raycast to target
            Vector3 startPos = ballInfo.Position;
            Vector3 distance = targetPos - startPos;
            bool directLine = !Physics.Raycast(startPos, distance.normalized, distance.magnitude, invalidAimLayer);
            float spikeTime = ballInfo.SkillValues.SkillToValue(skills.SpikePower, ballInfo.SkillValues.SpikePower);
            if (directLine) {
                // If clear, spike
                ballInfo.SetSpikeTarget(targetPos, spikeTime, this);
            } else {
                // If not, pass
                ballInfo.SetPassTarget(targetPos, startPos.y, spikeTime, this);
            }
        }

        public void BlockAttempt() {
            // get a random value on the skill level scale
            float randValue = UnityEngine.Random.value * skillLevelMax;
            // skill check
            Debug.Log($"block attempt {randValue} vs {skills.Blocking}");
            if (randValue <= skills.Blocking) Block();
        }

        private void Block() {
            Vector3 targetPos = new Vector3(2 * -courtSide, 0.01f, transform.position.z);
            float blockHeight = 4;
            float blockDuration = 2;
            ballInfo.SetPassTarget(targetPos, blockHeight, blockDuration, this);
        }

        public void SetSkills(SkillsSO skills) {
            this.skills = skills;
        }

        public void SetCourtSide(int courtSide) {
            this.courtSide = courtSide;
        }

        public void OnSwitchSides() {
            SetCourtSide(-courtSide);
        }

        protected virtual void OnDrawGizmos() {}

        //---- PROPERTIES ----

        public SkillsSO Skills { get { return skills; } }
        public float SkillLevelMax { get { return skillLevelMax; } }
        public BallSO BallInfo { get { return ballInfo; } }
        public MatchInfoSO MatchInfo { get { return matchInfo; } }
        public StateMachine StateMachine { get { return stateMachine; } }
        public int CourtSide { get { return courtSide; } set { courtSide = value; } }
        public float CourtSideLength { get { return courtSideLength; } }
        public Vector3 MoveDir {
            get { return moveDir; }
            set { moveDir = value; }
        }
        public Transform LeftHandEnd { get { return leftHandEnd; } }
        public Ball Ball { get { return ball; } }
        public bool IsJumping { get { return isJumping; } }
        public float ReachHeight { get { return reachHeight; } }
        public float JumpFrames { get { return jumpFrames; } }
        public float SpikeFrames { get { return spikeFrames; } }
        public float AnimationFrameRate { get { return animationFrameRate; } }
        public Athlete Teammate { get { return teammate; } set { teammate = value; } }
        public List<Athlete> Opponents { get { return opponents; } set { opponents = value; } }
    }
}
