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
        private CollisionTriggerReporter bodyTrigger;
        private CollisionTriggerReporter spikeTrigger;
        private CollisionTriggerReporter blockTrigger;
        protected Animator animator;
        private Transform leftHandEnd;
        private Vector3 blockColliderCenter;

        protected virtual void Awake() {
            bodyTrigger = transform.Find("Body").GetComponent<CollisionTriggerReporter>();
            bodyTrigger.Active = true;
            bodyTrigger.DeactivateAfterTrigger = false;
            spikeTrigger = transform.Find("Spike").GetComponent<CollisionTriggerReporter>();
            blockTrigger = transform.Find("Block").GetComponent<CollisionTriggerReporter>();

            animator = GetComponentInChildren<Animator>();

            obstaclesLayer = LayerMask.GetMask("Obstacles");
            invalidAimLayer = LayerMask.GetMask("InvalidAim");

            SetupStateMachine();
        }

        protected virtual void Start() {
            if (skills != null) {
                moveSpeed = skills.MoveSpeed;
                float percentScale = skills.Height / defaultHeight;

                transform.localScale = new Vector3(percentScale, percentScale, percentScale);
                BoxCollider spikeCollider = (BoxCollider)spikeTrigger.TriggerCollider;
                reachHeight = (spikeCollider.center.y + spikeCollider.size.y / 2) * percentScale;
                BoxCollider blockCollider = (BoxCollider)blockTrigger.TriggerCollider;
                blockColliderCenter = blockCollider.center * percentScale;
            } else {
                Debug.LogAssertion($"No skills found for { this.name }");
            }

            if (courtSide == null) {
                Debug.LogAssertion($"No court side found for { this.name }");
            }
            
            leftHandEnd = transform.Find("Volleyball-Character").Find("CCM-Armature").Find("Pelvis").Find("Spine1").Find("Spine2").Find("Shoulder.L").Find("UpperArm.L").Find("LowerArm.L").Find("Hand.L").Find("Hand.L_end");
        }

        protected virtual void Update() {
            stateMachine.Update();
            
            if (!isJumping) {
                Move();
            }

if (Skills.AthleteName == "Jorge Luis Alayo Moliner") {
    if (spikeTrigger.Active != lastEnabledStatus) {
        lastEnabledStatus = spikeTrigger.Active;
        Debug.Log($"{Skills.AthleteName} changed collider enabled to {spikeTrigger.Active} at time {Time.time}");
    }
}
        }
private bool lastEnabledStatus = false;

        protected virtual void SetupStateMachine() {
            // State Machine
            stateMachine = new StateMachine();
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
            if (Mathf.Sign(ballInfo.Position.x) == courtSide.Value) {
                spikeTrigger.Active = true;
            } else {
                blockTrigger.Active = true;
            }
            bodyTrigger.Active = false;
        }

        public void OnJumpPeakEvent() {
            spikeTrigger.Active = false;
            blockTrigger.Active = false;
        }

        public void OnJumpCompletedEvent() {
            isJumping = false;
            bodyTrigger.Active = true;
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

        public Vector3 CalculatePassTarget(Athlete teammate) {
            Vector2 teammatePos = new Vector2(teammate.transform.position.x, teammate.transform.position.z);
            Vector2 aimLocation = ballInfo.SkillValues.AdjustedPassLocation(teammatePos, this);
            return new Vector3(aimLocation.x, 0f, aimLocation.y);
        }

        public void Pass(Vector3 targetPos, float height, float time) {
            if (ballInfo.LastStatType == StatTypes.Attack) {
                ballInfo.StatUpdate.Raise(this, StatTypes.Dig);
            }
            ballInfo.SetPassTarget(targetPos, height, time, this, StatTypes.None);
        }

        public void Spike(Vector3 targetPos) {
            SetSpikeTargetByType(targetPos, skills.SpikeSkill, skills.SpikePower, ballInfo.SkillValues.SpikePower, StatTypes.Attack);
        }

        private void SetSpikeTargetByType(Vector3 targetPos, float athleteSkill, float athleteSkillPower, MinMax skillPowerRange, StatTypes statType) {
            // Raycast to target
            Vector3 startPos = ballInfo.Position;
            Vector3 distance = targetPos - startPos;
            bool directLine = !Physics.Raycast(startPos, distance.normalized, distance.magnitude, invalidAimLayer);

            float spikeTime = ballInfo.SkillValues.SkillToValue(athleteSkillPower, skillPowerRange) * (1 - Mathf.Abs(spikeSpeedPenalty));
            bool skillCheck = UnityEngine.Random.value <= ballInfo.SkillValues.SkillToValue(athleteSkill, ballInfo.SkillValues.SpikeOverNet);
            if (directLine || (!directLine && !skillCheck)) {
                // If clear, set direct target
                ballInfo.SetSpikeTarget(targetPos, spikeTime, this, statType);
            } else {
                // If not, use pass with adjusted height, pending a skill check
                float netCrossingT = Mathf.Abs(startPos.x) / Mathf.Abs(targetPos.x - startPos.x);
                float heightAtNet = ballInfo.CalculateInFlightPosition(netCrossingT, startPos, targetPos, startPos.y).y;
                float requiredHeight = Mathf.Lerp(2.5f, 2.75f, Mathf.InverseLerp(1, 8, Mathf.Abs(transform.position.x))); // this should be based on net height
                float adjustedHeight = startPos.y + requiredHeight - heightAtNet;
                ballInfo.SetSpikeTarget(targetPos, spikeTime, this, statType, adjustedHeight);
                Debug.Log($"No direct line and skill check passed. Adjusted height to {adjustedHeight}");
            }
            // Debug.Log($"{skills.AthleteName} has {(directLine ? "a clear line." : "no direct path (pos: " + startPos + " target: " + targetPos + "), using an arc.")}");
            ballInfo.StatUpdate.Raise(this, statType);
        }

        public void SpikeFeint(Vector3 targetPos) {
            Pass(targetPos, feintHeight, feintTime);
        }

        public void BlockAttempt(Collider blockedObject) {
            if (blockedObject.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                Vector3 contactPoint = blockTrigger.TriggerCollider.ClosestPoint(ball.transform.position);
                // get a random value on the skill level scale
                float randValue = UnityEngine.Random.value * skillLevelMax;
                // skill check
                Debug.Log($"block attempt by {skills.AthleteName}: {randValue} vs {skills.Blocking} [{(randValue <= skills.Blocking ? "Blocked" : "Missed")}]");
                ballInfo.StatUpdate.Raise(this, StatTypes.BlockAttempt);
                if (randValue <= skills.Blocking) Block(contactPoint);
            }
        }

        public void ServeOverhandAnimation() {
            animator.Play("ServeOverhand");
        }

        public Vector2 LockTowardsTarget() {
            // Calculate the distance from start to target
            Vector2 startPos = new Vector2(ballInfo.StartPos.x, ballInfo.StartPos.z);
            Vector2 targetPos = new Vector2(ballInfo.TargetPos.x, ballInfo.TargetPos.z);
            float ballTravelDistance = Vector2.Distance(startPos, targetPos);
            
            // Define your distance range and adjustment range
            float minDistance = 0;   // Short passes (adjust based on your game scale)
            float maxDistance = 10;  // Long passes (adjust based on your game scale)
            
            float minAdjustment = 0; // Minimum player position adjustment
            float maxAdjustment = 0.5f; // Maximum player position adjustment
            
            // Calculate the adjustment using Lerp (clamped to the defined ranges)
            float t = Mathf.InverseLerp(minDistance, maxDistance, ballTravelDistance);
            float adjustmentDistance = Mathf.Lerp(minAdjustment, maxAdjustment, t);
            // Debug.Log($"{minDistance} to {maxDistance} of {ballTravelDistance} is {Mathf.InverseLerp(minDistance, maxDistance, ballTravelDistance)} with adjustment of {adjustmentDistance}");
            
            // Get direction from ball start to target
            Vector2 ballPath = (targetPos - startPos).normalized;
            
            // Move player toward ball origin based on calculated adjustment
            return targetPos - ballPath * adjustmentDistance;
        }

        private void Block(Vector3 contactPoint) {
            // Use the stored contact point for more accurate quality calculation
            Vector3 contactDirection = contactPoint - (transform.position + blockColliderCenter);
            float contactQuality = Vector3.Dot(contactDirection.normalized, Vector3.right * -courtSide.Value);
            contactQuality = Mathf.Clamp01(contactQuality);

            Vector3 spikeDir = contactPoint - ballInfo.StartPos;
            Vector3 spikeDirXZ = new Vector3(spikeDir.x, 0, spikeDir.z).normalized;
            Vector3 blockNormal = transform.forward;
            Vector3 reflectDir = Vector3.Reflect(spikeDirXZ, blockNormal);
            Vector3 bounceDir = new Vector3(reflectDir.x, 0, reflectDir.z).normalized;
            float contactAngle = Vector3.Angle(spikeDirXZ, -blockNormal);
            
            // Determine if it's a strong block (spike) or a soft block (pass)
            bool strongBlock = contactAngle <= 15;
            Debug.Log($"{contactAngle} <= 15? -> {(strongBlock ? "Strong" : "Weak")} Block at y = {contactPoint.y}");
            float powerReduction = 0.5f;
            float maxBlockHeight = 5;

            float targetDistance = Mathf.Lerp(2f, 4f, contactQuality) * (strongBlock ? 1 : (1 - powerReduction));
            Debug.Log($"Target Distance: Lerp(2,4,{contactQuality})={targetDistance}");
            Debug.DrawLine(new Vector3(ballInfo.StartPos.x, contactPoint.y, ballInfo.StartPos.z), contactPoint, Color.yellow, 10f, false);
            Debug.DrawLine(contactPoint, contactPoint + blockNormal  * 3, Color.red, 10f, false);
            Debug.Log($"Contact point height: {contactPoint.y:F2}");
            Debug.DrawLine(contactPoint, GetBlockTargetPos(contactPoint, bounceDir, targetDistance), Color.green, 10f, false);
            
            float blockDuration;
            if (strongBlock) {
                // Strong blocks are like spikes - faster and more direct
                // blockDuration = Mathf.Lerp(ballInfo.SkillValues.BlockPower.min, ballInfo.SkillValues.BlockPower.max, skills.BlockPower);
                SetSpikeTargetByType(GetBlockTargetPos(contactPoint, bounceDir, targetDistance), skills.Blocking, skills.BlockPower, ballInfo.SkillValues.BlockPower, StatTypes.Block);
            } else {
                // Weak blocks are like passes - slower and higher
                float blockHeight = Mathf.Lerp(ballInfo.Position.y, maxBlockHeight, contactQuality);
                blockDuration = Mathf.Lerp(1.5f, 2.5f, contactQuality);
                ballInfo.SetSpikeTarget(GetBlockTargetPos(contactPoint, bounceDir, targetDistance * powerReduction), blockDuration, this, StatTypes.Block, blockHeight);
            }
            
            // Reset hits for this team
            ballInfo.HitsForTeam = 0;
            
            // Log for debugging
            // Debug.Log($"Block by {skills.AthleteName}: Contact Point={lastBlockContactPoint}, AthletePos={transform.position}, ColliderPos={spikeBlockCollider.center}, " +
            //         $"Direction={contactDirection} Quality={contactQuality}, Angle={contactAngle} ({(strongBlock ? "Strong" : "Weak")}) " +
            //         $"Target={targetPos} (Distance={targetDistance}, Duration={blockDuration} " +
            //         $"{(!strongBlock ? "Height=" + Mathf.Lerp(ball.transform.position.y, maxBlockHeight, contactQuality) : "")})"
            // );
        }

        private Vector3 GetBlockTargetPos(Vector3 contactPoint, Vector3 bounceDir, float blockDistance) {
            Vector3 targetPosXZ = contactPoint + bounceDir * blockDistance;
            return new Vector3(targetPosXZ.x, 0.01f, targetPosXZ.z);            
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
            if (courtSide != null) {
                transform.rotation = Quaternion.LookRotation(Vector3.right * -courtSide.Value);
            }
        }

        protected virtual void OnDrawGizmos() {}

        //---- PROPERTIES ----

        public SkillsSO Skills { get { return skills; } }
        public float SkillLevelMax { get { return skillLevelMax; } }
        public BallSO BallInfo => ballInfo;
        public MatchInfoSO MatchInfo => matchInfo;
        public AthleteStatsSO AthleteStats => athleteStats;
        public float CourtSide => courtSide.Value;
        public float CourtSideLength { get { return courtSideLength; } }
        public StateMachine StateMachine => stateMachine;
        public Vector3 MoveDir {
            get { return moveDir; }
            set { moveDir = value; }
        }
        public Transform LeftHandEnd { get { return leftHandEnd; } }
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
        public CollisionTriggerReporter SpikeTrigger => spikeTrigger;
        public CollisionTriggerReporter BodyTrigger => bodyTrigger;
        public CollisionTriggerReporter BlockTrigger => blockTrigger;
    }
}
