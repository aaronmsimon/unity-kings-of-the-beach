using System;
using UnityEngine;
using KotB.StatePattern.AIStates;
using Cackenballz.Helpers;

namespace KotB.Actors
{
    public class AI : Athlete
    {
        private ServeState serveState;
        private DefenseState defenseState;
        private OffenseState offenseState;
        private DigReadyState digReadyState;
        private PostPointState postPointState;
        private NonServeState nonServeState;
        private ServePosState servePosState;
        private ReceiveServeState receiveServeState;
        private DefenseBlockerState defenseBlockerState;
        private DefenseDefenderState defenseDefenderState;
        private Vector3 targetPos;
        private float estimateRange;

        public event Action ReachedTargetPos;

        private float stoppingDistance = 0.1f;
        private float distToGiveUp = 1;

        protected override void Awake() {
            base.Awake();

            serveState = new ServeState(this);
            defenseState = new DefenseState(this);
            offenseState = new OffenseState(this);
            digReadyState = new DigReadyState(this);
            postPointState = new PostPointState(this);
            nonServeState = new NonServeState(this);
            servePosState = new ServePosState(this);
            receiveServeState = new ReceiveServeState(this);
            defenseBlockerState = new DefenseBlockerState(this);
            defenseDefenderState = new DefenseDefenderState(this);

            stateMachine.ChangeState(postPointState);

            estimateRange = BallInfo.BallRadius * 2;
        }

        protected override void Update() {
            Vector3 moveDiff = targetPos - transform.position;

            if (moveDiff.sqrMagnitude > stoppingDistance * stoppingDistance) {
                moveDir = moveDiff.normalized;
            } else {
                moveDir = Vector3.zero;
                ReachedTargetPos?.Invoke();
            }

            base.Update();
        }

        public Vector3 SetTargetToGiveUp(float minDist, float giveUpDistance) {
            Vector3 ballTargetPos = BallInfo.TargetPos;
            Vector3 pos = transform.position;
            Vector3 direction = ballTargetPos - pos;

            // Check intersections with each boundary
            if (ballTargetPos.x > courtSideLength / 2 * courtSide.Value + 4) minDist = Mathf.Min(minDist, (courtSideLength / 2 * courtSide.Value + 4 - giveUpDistance - pos.x) / direction.x);
            if (ballTargetPos.x < courtSideLength / 2 * courtSide.Value - 4) minDist = Mathf.Min(minDist, (courtSideLength / 2 * courtSide.Value - 4 + giveUpDistance - pos.x) / direction.x);
            if (ballTargetPos.z > courtSideLength / 2) minDist = Mathf.Min(minDist, (courtSideLength / 2 - giveUpDistance - pos.z) / direction.z);
            if (ballTargetPos.z < -courtSideLength / 2) minDist = Mathf.Min(minDist, (-courtSideLength / 2 + giveUpDistance - pos.z) / direction.z);

            // Compute final stopping position
            return pos + direction * minDist;
        }

        public bool JudgeInBounds() {
            // get a random value on the skill level scale
            float randValue = UnityEngine.Random.value * SkillLevelMax;
            // skill check
            bool accurateEstimate = randValue <= Skills.InBoundsJudgement;
            // get actual in bounds value
            bool actualInBounds = BallInfo.IsInBounds(BallInfo.TargetPos);
            // check if it's close
            bool closeInBounds =
                (Mathf.Abs(BallInfo.TargetPos.x) >= CourtSideLength - estimateRange && Mathf.Abs(BallInfo.TargetPos.x) <= CourtSideLength + estimateRange) ||
                (Mathf.Abs(BallInfo.TargetPos.z) >= CourtSideLength / 2 - estimateRange && Mathf.Abs(BallInfo.TargetPos.z) <= CourtSideLength / 2 + estimateRange);
            //                                                                             ↓ not close - anyone knows the right result
            return closeInBounds ? (accurateEstimate ? actualInBounds : !actualInBounds) : actualInBounds;
            //     ↑ if it's close  ↑ if passed skill chk,  correct ↑    ↑ otherwise, wrong
            
        }

        public bool MyBall() {
            Athlete teammate = MatchInfo.GetTeammate(this);
            if (teammate != null) {
                float myDistToBall = (BallInfo.TargetPos - transform.position).sqrMagnitude;
                float teammateDistToBall = (BallInfo.TargetPos - teammate.transform.position).sqrMagnitude;
                return (myDistToBall < teammateDistToBall) || (myDistToBall == teammateDistToBall && Skills.PlayerPosition == PositionType.Defender);
            } else {
                return true;
            }
        }

        public void OnBallHitGround() {
            stateMachine.ChangeState(postPointState);
            targetPos = transform.position;
        }

        protected override void OnDrawGizmos() {
            base.OnDrawGizmos();
            // Dig Range
            if (skills != null) {
                GizmoHelpers.DrawGizmoCircle(transform.position, skills.DigRange, Color.red, 12);
            }
        }

        //---- PROPERTIES ----
        
        public ServeState ServeState { get { return serveState; } }
        public DefenseState DefenseState { get { return defenseState; } }
        public OffenseState OffenseState { get { return offenseState; } }
        public DigReadyState DigReadyState { get { return digReadyState; } }
        public PostPointState PostPointState { get { return postPointState; } }
        public NonServeState NonServeState { get { return nonServeState; } }
        public ServePosState ServePosState { get { return servePosState; } }
        public ReceiveServeState ReceiveServeState { get { return receiveServeState; } }
        public DefenseBlockerState DefenseBlockerState => defenseBlockerState;
        public DefenseDefenderState DefenseDefenderState => defenseDefenderState;
        public Vector3 OffensePos { get { return new Vector3(skills.OffenseXPos * courtSide.Value, 0.01f, transform.position.z); } }
        public Vector3 TargetPos
        {
            get => targetPos;
            set => targetPos = value;
        }
        public float DistToGiveUp => distToGiveUp;
    }
}
