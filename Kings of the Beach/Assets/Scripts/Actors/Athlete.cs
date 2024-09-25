using System;
using UnityEngine;
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
        public event Action MatchChangeToServeState;

        public delegate void ServeBall(Vector3 aimPoint, float duration);
        public ServeBall OnServeBallAction;

        private bool canBump;
        private float unlockDelay = 0.25f;

        protected Ball ball;
        protected float courtSideLength = 8;
        private float skillLevelMax = 10;
        protected Vector3 moveDir;
        protected float bumpTimer;
        protected Vector3 bumpTarget;
        protected bool canUnlock;
        protected float unlockTimer;
        // protected Vector3 serveTarget;

        protected virtual void Start() {
            if (skills == null) {
                Debug.LogAssertion($"No skills found for { this.name }");
            }

            canBump = false;
            bumpTimer = 0;
        }

        protected virtual void Update() {
        //     switch (athleteState) {
        //         case AthleteState.Normal:
                    Move();
        //             break;
        //         case AthleteState.Locked:
        //             Bump();
        //             TryUnlock();
        //             break;
        //         default:
        //             Debug.LogWarning("Athlete State unhandled.");
        //             break;
        //     }
        }

        protected virtual void OnTriggerEnter(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                this.ball = ball;
                OnServeBallAction = ball.Serve;
                canBump = true;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                canBump = false;
            }
        }

        protected virtual void Move() {
            bool canMove = !Physics.Raycast(transform.position + Vector3.up * 0.5f, moveDir, out RaycastHit hit, 0.5f, obstaclesLayer);
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, moveDir, Color.red);
            if (canMove) {
                transform.position += moveDir * skills.MoveSpeed * Time.deltaTime;
            }

            skills.Position = transform.position;
        }

        public void Bump() {
            bumpTimer -= Time.deltaTime;
            if (canBump && bumpTimer > 0 && this.ball != null) {
                this.ball.Bump(bumpTarget, 12, 2);
                canUnlock = true;
                unlockTimer = unlockDelay;
                canBump = false;
                ballInfo.HitsForTeam += 1;
                Debug.Log("Hits: " + ballInfo.HitsForTeam);
                ballInfo.lastPlayerToHit = this;
            }
        }

        // public void Serve() {
        //     if (this.ball != null) {
        //         this.ball.Serve(serveTarget, 1.5f);
        //         Debug.Log("Ball Served");
        //     }
        // }

        private void SetCourtSide(int courtSide) {
            this.courtSide = courtSide;
        }

        private void SwitchCourtSide() {
            courtSide *= -1;
        }

        //---- EVENT LISTENERS ----

        public void OnBallHitGround() {
            BallHitGround?.Invoke();
        }

        public void OnMatchChangeToServeState() {
            MatchChangeToServeState?.Invoke();
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
    }
}
