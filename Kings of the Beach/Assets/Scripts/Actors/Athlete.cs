using System;
using UnityEngine;
using KotB.StatePattern;
using KotB.Match;
using RoboRyanTron.Unite2017.Variables;
using KotB.Items;
using KotB.Stats;

namespace KotB.Actors
{
    public abstract class Athlete : MonoBehaviour
    {
        [Header("Skills")]
        [SerializeField] protected SkillsSO skills;

        [Header("Scriptable Objects")]
        [SerializeField] protected BallSO ballInfo;
        [SerializeField] protected MatchInfoSO matchInfo;
        [SerializeField] private AthleteStatsSO athleteStats;

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
        private float actionFrames = 9;
        protected float serveOverhandFrames = 34;
        protected float serveOverhandContactFrames = 25;
        protected float animationFrameRate = 24;

        private float reachHeight;
        private float spikeSpeedPenalty = 0;
        private float feintHeight = 5;
        private float feintTime = 1;

        // caching
        private float moveSpeed;
        private CapsuleCollider capCollider;
        private SphereCollider spikeBlockCollider;
        protected Animator animator;
        private Transform leftHandEnd;


        protected virtual void Awake() {
            stateMachine = new StateMachine();

            capCollider = GetComponent<CapsuleCollider>();
            spikeBlockCollider = GetComponent<SphereCollider>();
            animator = GetComponentInChildren<Animator>();

            obstaclesLayer = LayerMask.GetMask("Obstacles");
            invalidAimLayer = LayerMask.GetMask("InvalidAim");
        }

        protected virtual void Start() {
            if (skills != null) {
                moveSpeed = skills.MoveSpeed;
                float percentScale = skills.Height / defaultHeight;

                transform.localScale = new Vector3(percentScale, percentScale, percentScale);
                reachHeight = (spikeBlockCollider.center.y + spikeBlockCollider.radius) * percentScale;
            } else {
                Debug.LogAssertion($"No skills found for { this.name }");
            }

            leftHandEnd = transform.Find("Volleyball-Character").Find("CCM-Armature").Find("Pelvis").Find("Spine1").Find("Spine2").Find("Shoulder.L").Find("UpperArm.L").Find("LowerArm.L").Find("Hand.L").Find("Hand.L_end");
        }

        protected virtual void Update() {
            stateMachine.Update();
            
            if (!isJumping) {
                Move();
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
            
            if (moveDir != Vector3.zero) {
                // Project moveDir onto the XZ plane to remove any Y-axis influence
                moveDir.y = 0;
                moveDir.Normalize();

                // Calculate the target rotation around the Y-axis
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);

                // Preserve the current X and Z rotation, modifying only Y
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                    Quaternion.Euler(0, targetRotation.eulerAngles.y, 0), 
                                                    rotateSpeed * Time.deltaTime);
            }

            skills.Position = transform.position;
        }

        public void OnJumpEvent() {
            spikeBlockCollider.enabled = true;
            capCollider.enabled = false;
        }

        public void OnJumpPeakEvent() {
            spikeBlockCollider.enabled = false;
        }

        public void OnJumpCompletedEvent() {
            isJumping = false;
            capCollider.enabled = true;
        }

        public void PerformJump() {
            isJumping = true;
            FaceOpponent();
            if (courtSide.Value == Mathf.Sign(ballInfo.Position.x)) {
                animator.Play("Spike");
            } else {
                animator.Play("Block");
            }
        }

        public void Pass(Vector3 targetPos, float height, float time) {
            if (ballInfo.LastStatType == StatTypes.Attack) {
                ballInfo.StatUpdate.Raise(this, StatTypes.Dig);
            }
            ballInfo.SetPassTarget(targetPos, height, time, this, StatTypes.None);
        }

        public void Spike(Vector3 targetPos) {
            // Raycast to target
            Vector3 startPos = ballInfo.Position;
            Vector3 distance = targetPos - startPos;
            bool directLine = !Physics.Raycast(startPos, distance.normalized, distance.magnitude, invalidAimLayer);
            float spikeTime = ballInfo.SkillValues.SkillToValue(skills.SpikePower, ballInfo.SkillValues.SpikePower) * (1 - Mathf.Abs(spikeSpeedPenalty));
            // Debug.Log($"spikeTime (skill): {ballInfo.SkillValues.SkillToValue(skills.SpikePower, ballInfo.SkillValues.SpikePower)} * (1 - Mathf.Abs({spikeSpeedPenalty})) = {spikeTime}");
            float skillCheckRand = UnityEngine.Random.value;
            bool skillCheck = skillCheckRand <= ballInfo.SkillValues.SkillToValue(skills.SpikeSkill, ballInfo.SkillValues.SpikeOverNet);
            if (directLine || (!directLine && !skillCheck)) {
                // If clear, spike
                if (directLine) {
                    Debug.Log("Spike had a clear path");
                } else {
                    Debug.Log($"No clear line for spike. Spike Skill: {Skills.SpikeSkill}. Skill check failed ({skillCheckRand} > {ballInfo.SkillValues.SkillToValue(skills.SpikeSkill, ballInfo.SkillValues.SpikeOverNet)})");
                }
                ballInfo.SetSpikeTarget(targetPos, spikeTime, this, StatTypes.Attack);
            } else {
                Debug.Log($"No clear line for spike. Spike Skill: {Skills.SpikeSkill}. Skill check passed ({skillCheckRand} <= {ballInfo.SkillValues.SkillToValue(skills.SpikeSkill, ballInfo.SkillValues.SpikeOverNet)})");
                // If not, use pass with adjusted height, pending a skill check
                float netCrossingT = Mathf.Abs(startPos.x) / Mathf.Abs(targetPos.x - startPos.x);
                float heightAtNet = ballInfo.CalculateInFlightPosition(netCrossingT, startPos, targetPos, startPos.y).y;
                float requiredHeight = 2.5f;
                float adjustedHeight = startPos.y + requiredHeight - heightAtNet;
                ballInfo.SetPassTarget(targetPos, adjustedHeight, spikeTime, this, StatTypes.Attack);
            }
            Debug.Log($"{skills.AthleteName} has {(directLine ? "a clear line to a clean spike." : "no direct path (pos: " + startPos + " target: " + targetPos + "), using an arc shot.")}");
            ballInfo.StatUpdate.Raise(this, StatTypes.Attack);
        }

        public void SpikeFeint(Vector3 targetPos) {
            Pass(targetPos, feintHeight, feintTime);
        }

        public void BlockAttempt() {
            // get a random value on the skill level scale
            float randValue = UnityEngine.Random.value * skillLevelMax;
            // skill check
            Debug.Log($"block attempt by {skills.AthleteName}: {randValue} vs {skills.Blocking} [{(randValue <= skills.Blocking ? "Blocked" : "Missed")}]");
            ballInfo.StatUpdate.Raise(this, StatTypes.BlockAttempt);
            // just in case - avoid double blocks
            spikeBlockCollider.enabled = false;
            if (randValue <= skills.Blocking) Block();
        }

        public void ServeOverhandAnimation() {
            animator.Play("ServeOverhand");
        }

        private void Block() {
            Vector3 targetPos = new Vector3(2 * -courtSide.Value, 0.01f, transform.position.z);
            float blockHeight = 4;
            float blockDuration = 2;
            ballInfo.SetPassTarget(targetPos, blockHeight, blockDuration, this, StatTypes.Block);
            ballInfo.HitsForTeam = 0;
            ballInfo.StatUpdate.Raise(this, StatTypes.Block);
        }

        public void SetSkills(SkillsSO skills) {
            this.skills = skills;
        }

        public void SetCourtSide(FloatVariable courtSide) {
            this.courtSide = courtSide;
        }

        public void SetStats(AthleteStatsSO athleteStats) {
            this.athleteStats = athleteStats;
        }

        public void FaceOpponent() {
            transform.rotation = Quaternion.LookRotation(Vector3.right * -courtSide.Value);
        }

        protected virtual void OnDrawGizmos() {}

        //---- PROPERTIES ----

        public SkillsSO Skills { get { return skills; } }
        public float SkillLevelMax { get { return skillLevelMax; } }
        public BallSO BallInfo => ballInfo;
        public MatchInfoSO MatchInfo => matchInfo;
        public AthleteStatsSO AthleteStats => athleteStats;
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
        public float ActionFrames { get { return actionFrames; } }
        public float ServeOverhandContactFrames => serveOverhandContactFrames;
        public float AnimationFrameRate { get { return animationFrameRate; } }
        public float SpikeSpeedPenalty { set { spikeSpeedPenalty = value; } }
        public float ReceiveServeXPos => receiveServeXPos;
        public Vector3 ServeDefPos {
            get {
                return new Vector3(receiveServeXPos * courtSide.Value, 0.01f, skills.DefensePos.y * (matchInfo.GetTeam(this).IsCaptain(this) ? 1 : -1));
            }
        }
        public float NoMansLand { get { return noMansLand; } }
        public SphereCollider SpikeBlockCollider { get { return spikeBlockCollider; } }
    }
}
