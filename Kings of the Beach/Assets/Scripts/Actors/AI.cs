using UnityEngine;
using KotB.StatePattern;
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

        private Athlete teammate;

        protected override void Awake() {
            base.Awake();

            serveState = new ServeState(this);
            defenseState = new DefenseState(this);
            offenseState = new OffenseState(this);
            digReadyState = new DigReadyState(this);
            postPointState = new PostPointState(this);

            stateMachine.ChangeState(postPointState);
        }

        public Vector3 GetMyDefensivePosition(Vector3 defensivePos) {
            return new Vector3(defensivePos.x * courtSide, defensivePos.y, defensivePos.z * (Mathf.Sign(defensivePos.z) * Mathf.Sign(teammate.Skills.Position.z) * -1));
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
        public Athlete Teammate { get { return teammate; } set { teammate = value; } }
        public Vector3 DefensePos {
            get {
                float defenseZPos;
                // If no teammate (debugging but potentially practice, too)
                if (teammate != null) {
                    if (teammate.GetComponent<Player>() != null || teammate.Skills.PlayerPosition == PositionType.Defender) {
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
