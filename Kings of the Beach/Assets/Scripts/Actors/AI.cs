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

        public Vector3 TargetPos { get; set; }

        public event Action ReachedTargetPos;

        private float stoppingDistance = 0.1f;

        protected override void Awake() {
            base.Awake();

            serveState = new ServeState(this);
            defenseState = new DefenseState(this);
            offenseState = new OffenseState(this);
            digReadyState = new DigReadyState(this);
            postPointState = new PostPointState(this);
            nonServeState = new NonServeState(this);
            servePosState = new ServePosState(this);

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
        public NonServeState NonServeState { get { return nonServeState; } }
        public ServePosState ServePosState { get { return servePosState; } }
        public Vector3 OffensePos { get { return new Vector3(skills.OffenseXPos * courtSide.Value, 0.01f, transform.position.z); } }
    }
}
