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

        protected Ball ball;
        protected float courtSideLength = 8;
        private float skillLevelMax = 10;
        protected Vector3 moveDir;
        protected Vector3 bumpTarget;

        protected virtual void Start() {
            if (skills == null) {
                Debug.LogAssertion($"No skills found for { this.name }");
            }
        }

        protected virtual void Update() {
            Move();
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

        protected virtual void Move() {
            bool canMove = !Physics.Raycast(transform.position + Vector3.up * 0.5f, moveDir, out RaycastHit hit, 0.5f, obstaclesLayer);
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, moveDir, Color.red);
            if (canMove) {
                transform.position += moveDir * skills.MoveSpeed * Time.deltaTime;
            }

            skills.Position = transform.position;
        }

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
