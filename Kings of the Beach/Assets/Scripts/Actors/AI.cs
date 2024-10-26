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

        public Vector3 TargetPos { get; set; }
        public Athlete Teammate { get; set; }

        public event Action ReachedTargetPos;

        private float stoppingDistance = 0.1f;

        protected override void Awake() {
            base.Awake();

            serveState = new ServeState(this);
            defenseState = new DefenseState(this);
            offenseState = new OffenseState(this);
            digReadyState = new DigReadyState(this);
            postPointState = new PostPointState(this);

            stateMachine.ChangeState(postPointState);
        }

        protected override void Update() {
            Vector3 moveDiff = TargetPos - transform.position;

            if (moveDiff.sqrMagnitude > stoppingDistance * stoppingDistance) {
                moveDir = moveDiff.normalized;
            } else {
                moveDir = Vector3.zero;
                ReachedTargetPos?.Invoke();
            }

            base.Update();
        }

        public Vector3 GetMyDefensivePosition(Vector3 defensivePos) {
            return new Vector3(skills.DefensePos.x * courtSide, defensivePos.y, skills.DefensePos.y * (Mathf.Sign(defensivePos.z) * Mathf.Sign(Teammate.Skills.Position.z) * -1));
        }

        public void OnBallHitGround() {
            stateMachine.ChangeState(postPointState);
            TargetPos = transform.position;
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
        public Vector3 DefensePos {
            get {
                float defenseZPos;
                // If no teammate (debugging but potentially practice, too)
                if (Teammate != null) {
                    if (Teammate.GetComponent<Player>() != null || Teammate.Skills.PlayerPosition == PositionType.Defender) {
                        defenseZPos = GetMyDefensivePosition(transform.position).z;
                    } else {
                        defenseZPos = transform.position.z;
                    }
                } else {
                    defenseZPos = 0;
                }

                return new Vector3(skills.DefensePos.x * courtSide, 0.01f, defenseZPos);
            }
        }
        public Vector3 OffensePos { get { return new Vector3(skills.OffenseXPos * courtSide, 0.01f, transform.position.z); } }
    }
}
