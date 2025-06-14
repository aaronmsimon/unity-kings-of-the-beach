using System;
using UnityEngine;
using KotB.StatePattern;
using KotB.StatePattern.AIStates;
using Cackenballz.Helpers;

namespace KotB.Actors
{
    public class AI : Athlete
    {
        private Vector3 targetPos;
        private float estimateRange;

        public event Action ReachedTargetPos;

        private float stoppingDistance = 0.1f;
        private float distToGiveUp = 1;

        private EventPredicate ballHitGroundPredicate;
        private EventPredicate ballServedPredicate;
        private EventPredicate targetSetPredicate;
        private EventPredicate matchToServePredicate;

        public EventPredicate ServeTargetPosReachedPredicate { get; private set; }
        public EventPredicate ServeToDefensePredicate { get; private set; }
        public EventPredicate DigToOffensePredicate { get; private set; }
        public EventPredicate DigToDefensePredicate { get; private set; }

        protected override void Awake() {
            base.Awake();

            estimateRange = BallInfo.BallRadius * 2;

            SetupStateMachine();
        }

        private void OnDisable() {
            ballInfo.BallServed -= ballServedPredicate.Trigger;
            ballInfo.TargetSet -= targetSetPredicate.Trigger;
            matchInfo.TransitionToServeState -= matchToServePredicate.Trigger;

            ballHitGroundPredicate.Cleanup();
            ballServedPredicate.Cleanup();
            targetSetPredicate.Cleanup();
            matchToServePredicate.Cleanup();
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

        public float GetTimeToContactHeight(float contactHeight, float height, float start, float end, float duration)
        {
            float a = -4 * height;
            float b = 4 * height - start + end;
            float c = start - contactHeight;

            float discriminant = b * b - 4 * a * c;

            if (discriminant >= 0)
            {
                float sqrtDiscriminant = Mathf.Sqrt(discriminant);
                float t1 = (-b + sqrtDiscriminant) / (2 * a);
                float t2 = (-b - sqrtDiscriminant) / (2 * a);

                if (t1 >= 0.5f && t1 <= 1)
                    return t1 * duration;
                else if (t2 >= 0.5f && t2 <= 1)
                    return t2 * duration;
            }

            // Debug.LogError($"No real solution for spikePos={spikePos}, height={height}, start={start}, end={end}, duration={duration} leading to discriminant={discriminant}");
            return -1;
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

        private bool MyBall() {
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
            ballHitGroundPredicate.Trigger();
            targetPos = transform.position;
        }

        private void SetupStateMachine() {
            // State Machine
            stateMachine = new StateMachine();

            // Declare States
            var serveState = new ServeState(this);
            var defenseState = new DefenseState(this);
            var offenseState = new OffenseState(this);
            var digReadyState = new DigReadyState(this);
            var postPointState = new PostPointState(this);
            var nonServeState = new NonServeState(this);
            var servePosState = new ServePosState(this);
            var receiveServeState = new ReceiveServeState(this);
            var defenseBlockerState = new DefenseBlockerState(this);
            var defenseDefenderState = new DefenseDefenderState(this);

            // Declare Event Predicates
            ballHitGroundPredicate = new EventPredicate(stateMachine);
            
            ballServedPredicate = new EventPredicate(stateMachine);
            targetSetPredicate = new EventPredicate(stateMachine);
            matchToServePredicate = new EventPredicate(stateMachine);
            ServeTargetPosReachedPredicate = new EventPredicate(stateMachine);
            ServeToDefensePredicate = new EventPredicate(stateMachine);
            DigToOffensePredicate = new EventPredicate(stateMachine);
            DigToDefensePredicate = new EventPredicate(stateMachine);

            // Subscribe Event Predicates to Events
            ballInfo.BallServed += ballServedPredicate.Trigger;
            ballInfo.TargetSet += targetSetPredicate.Trigger;
            matchInfo.TransitionToServeState += matchToServePredicate.Trigger;

            // Declare Default Profile & Transitions
            TransitionProfile defaultProfile = new TransitionProfile("Default");

                // Setup Transitions
                defaultProfile.AddAnyTransition(postPointState, ballHitGroundPredicate);
                defaultProfile.AddTransition(postPointState, servePosState, new AndPredicate(matchToServePredicate, new FuncPredicate(() => matchInfo.GetServer() == this)));
                defaultProfile.AddTransition(postPointState, nonServeState, new AndPredicate(matchToServePredicate, new FuncPredicate(() => matchInfo.Teams[matchInfo.TeamServeIndex] == matchInfo.GetTeam(this) && matchInfo.GetServer() != this)));
                defaultProfile.AddTransition(postPointState, receiveServeState, new AndPredicate(matchToServePredicate, new FuncPredicate(() => matchInfo.Teams[matchInfo.TeamServeIndex] != matchInfo.GetTeam(this))));
                defaultProfile.AddTransition(servePosState, serveState, ServeTargetPosReachedPredicate);
                defaultProfile.AddTransition(nonServeState, defenseState, ballServedPredicate);
                defaultProfile.AddTransition(receiveServeState, digReadyState, new AndPredicate(targetSetPredicate, new FuncPredicate(() => MyBall()), new FuncPredicate(() => JudgeInBounds())));
                defaultProfile.AddTransition(receiveServeState, offenseState, new AndPredicate(targetSetPredicate, new FuncPredicate(() => !MyBall())));
                defaultProfile.AddTransition(serveState, defenseState, ServeToDefensePredicate);
                defaultProfile.AddTransition(defenseState, defenseBlockerState, new FuncPredicate(() => skills.PlayerPosition == PositionType.Blocker));
                defaultProfile.AddTransition(defenseState, defenseDefenderState, new FuncPredicate(() => skills.PlayerPosition == PositionType.Defender));
                defaultProfile.AddTransition(digReadyState, offenseState, DigToOffensePredicate);
                defaultProfile.AddTransition(digReadyState, defenseState, DigToDefensePredicate);
                defaultProfile.AddTransition(defenseBlockerState, digReadyState, new AndPredicate(
                    targetSetPredicate,
                    new FuncPredicate(() => Mathf.Sign(ballInfo.TargetPos.x) == courtSide.Value),
                    new FuncPredicate(() => ballInfo.Possession == courtSide.Value)
                ));
                defaultProfile.AddTransition(defenseDefenderState, digReadyState, new AndPredicate(
                    targetSetPredicate,
                    new FuncPredicate(() => Mathf.Sign(ballInfo.TargetPos.x) == courtSide.Value),
                    new FuncPredicate(() => JudgeInBounds())
                ));
                defaultProfile.AddTransition(offenseState, digReadyState, new AndPredicate(targetSetPredicate, new FuncPredicate(() => Mathf.Sign(ballInfo.TargetPos.x) == courtSide.Value)));
                defaultProfile.AddTransition(offenseState, defenseState, new AndPredicate(targetSetPredicate, new FuncPredicate(() => Mathf.Sign(ballInfo.TargetPos.x) != courtSide.Value)));
                defaultProfile.SetStartingState(postPointState);

                stateMachine.AddProfile(defaultProfile);
        }

        protected override void OnDrawGizmos() {
            base.OnDrawGizmos();
            // Dig Range
            if (skills != null) {
                GizmoHelpers.DrawGizmoCircle(transform.position, skills.DigRange, Color.red, 12);
            }
        }

        //---- PROPERTIES ----
        
        public Vector3 TargetPos
        {
            get => targetPos;
            set => targetPos = value;
        }
        public float DistToGiveUp => distToGiveUp;
    }
}
