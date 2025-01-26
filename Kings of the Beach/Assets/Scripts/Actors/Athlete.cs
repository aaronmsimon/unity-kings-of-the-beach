using System;
using UnityEngine;
using KotB.StatePattern;
using KotB.Match;
using RoboRyanTron.Unite2017.Variables;
using KotB.Items;

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
        [SerializeField] protected FloatVariable courtSide;

        protected Ball ball;
        protected StateMachine stateMachine;
        protected float courtSideLength = 8;
        protected Vector3 moveDir;
        protected bool isJumping;
        protected bool feint;
        protected float receiveServeXPos = 5.5f;

        private LayerMask obstaclesLayer;
        private LayerMask invalidAimLayer;
        private float defaultHeight = 1.9f;
        private float noMansLand = 0.5f;
        private float skillLevelMax = 10;
        private float jumpFrames = 7;
        private float spikeFrames = 9;
        private float spikeFallFrames = 8;
        private float blockFrames = 9;
        private float blockFallFrames = 8;
        protected float animationFrameRate = 24;
        private float jumpAnimationTime;
        private float reachHeight;
        private float spikeSpeedPenalty = 0;
        private float feintHeight = 5;
        private float feintTime = 1;

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

            obstaclesLayer = LayerMask.GetMask("Obstacles");
            invalidAimLayer = LayerMask.GetMask("InvalidAim");
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
            jumpAnimationTime += Time.deltaTime;

            // Spiking
            if (animator.GetBool("isSpike")) {
                if (jumpAnimationTime >= jumpFrames / animationFrameRate && jumpAnimationTime < (jumpFrames + spikeFrames) / animationFrameRate) {
                    sphereCollider.enabled = true;
                    capCollider.enabled = false;
                } else if (jumpAnimationTime >= (jumpFrames + spikeFrames) / animationFrameRate && jumpAnimationTime < (jumpFrames + spikeFrames + spikeFallFrames) / animationFrameRate) {
                    sphereCollider.enabled = false;
                } else if (jumpAnimationTime >= (jumpFrames + spikeFrames + spikeFallFrames) / animationFrameRate) {
                    animator.SetBool("isSpike", false);
                    isJumping = false;
                    capCollider.enabled = true;
                }
            }

            // Blocking
            if (animator.GetBool("isBlock")) {
                if (jumpAnimationTime >= jumpFrames / animationFrameRate && jumpAnimationTime < (jumpFrames + blockFrames) / animationFrameRate) {
                    sphereCollider.enabled = true;
                    capCollider.enabled = false;
                } else if (jumpAnimationTime >= (jumpFrames + blockFrames) / animationFrameRate && jumpAnimationTime < (jumpFrames + blockFrames + blockFallFrames) / animationFrameRate) {
                    sphereCollider.enabled = false;
                } else if (jumpAnimationTime >= (jumpFrames + blockFrames + blockFallFrames) / animationFrameRate) {
                    animator.SetBool("isBlock", false);
                    isJumping = false;
                    capCollider.enabled = true;
                }
            }
        }

        public void PerformJump() {
            isJumping = true;
            jumpAnimationTime = 0;
            transform.forward = new Vector3(-courtSide.Value, 0, 0);
            if (courtSide.Value == Mathf.Sign(ballInfo.Position.x)) {
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
            float spikeTime = ballInfo.SkillValues.SkillToValue(skills.SpikePower, ballInfo.SkillValues.SpikePower) * (1 - Mathf.Abs(spikeSpeedPenalty));
            // Debug.Log($"spikeTime (skill): {ballInfo.SkillValues.SkillToValue(skills.SpikePower, ballInfo.SkillValues.SpikePower)} * (1 - Mathf.Abs({spikeSpeedPenalty})) = {spikeTime}");
            if (directLine) {
                // If clear, spike
                ballInfo.SetSpikeTarget(targetPos, spikeTime, this);
            } else {
                // If not, pass
                ballInfo.SetPassTarget(targetPos, startPos.y, spikeTime, this);
            }
        }

        public void SpikeFeint(Vector3 targetPos) {
            Pass(targetPos, feintHeight, feintTime);
        }

        public void BlockAttempt() {
            // get a random value on the skill level scale
            float randValue = UnityEngine.Random.value * skillLevelMax;
            // skill check
            Debug.Log($"block attempt {randValue} vs {skills.Blocking} [{(randValue <= skills.Blocking ? "Blocked" : "Missed")}]");
            if (randValue <= skills.Blocking) Block();
        }

        private void Block() {
            Vector3 targetPos = new Vector3(2 * -courtSide.Value, 0.01f, transform.position.z);
            float blockHeight = 4;
            float blockDuration = 2;
            ballInfo.SetPassTarget(targetPos, blockHeight, blockDuration, this);
        }

        public void SetSkills(SkillsSO skills) {
            this.skills = skills;
        }

        public void SetCourtSide(FloatVariable courtSide) {
            this.courtSide = courtSide;
        }

        protected virtual void OnDrawGizmos() {}

        //---- PROPERTIES ----

        public SkillsSO Skills { get { return skills; } }
        public float SkillLevelMax { get { return skillLevelMax; } }
        public BallSO BallInfo { get { return ballInfo; } }
        public MatchInfoSO MatchInfo { get { return matchInfo; } }
        public StateMachine StateMachine { get { return stateMachine; } }
        public float CourtSide => courtSide.Value;
        public float CourtSideLength { get { return courtSideLength; } }
        public Vector3 MoveDir {
            get { return moveDir; }
            set { moveDir = value; }
        }
        public Transform LeftHandEnd { get { return leftHandEnd; } }
        public Ball Ball { get { return ball; } }
        public bool IsJumping { get { return isJumping; } }
        public bool Feint { get { return feint; } set { feint = value; } }
        public float ReachHeight { get { return reachHeight; } }
        public float JumpFrames { get { return jumpFrames; } }
        public float SpikeFrames { get { return spikeFrames; } }
        public float AnimationFrameRate { get { return animationFrameRate; } }
        public float SpikeSpeedPenalty { set { spikeSpeedPenalty = value; } }
        public float ReceiveServeXPos => receiveServeXPos;
        public Vector3 ServeDefPos {
            get {
                return new Vector3(receiveServeXPos * courtSide.Value, 0.01f, skills.DefensePos.y * (matchInfo.GetTeam(this).IsCaptain(this) ? 1 : -1));
            }
        }
    }
}
