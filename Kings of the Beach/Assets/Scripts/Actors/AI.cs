using UnityEngine;
using KotB.StatePattern;
using KotB.StatePattern.AIStates;
using Cackenballz.Helpers;

namespace KotB.Actors
{
    public class AI : Athlete
    {
        private StateMachine aiStateMachine;
        private ServeState serveState;
        private DefenseState defenseState;
        private OffenseState offenseState;
        private DigReadyState digReadyState;
        private PostPointState postPointState;

        private Athlete teammate;

        private float passRangeMin = 0.8f;
        private float passRangeMax = 2.5f;

        protected override void Start() {
            base.Start();

            aiStateMachine = new StateMachine();
            serveState = new ServeState(this);
            defenseState = new DefenseState(this);
            offenseState = new OffenseState(this);
            digReadyState = new DigReadyState(this);
            postPointState = new PostPointState(this);

            aiStateMachine.ChangeState(postPointState);
        }

        protected override void Update() {
            base.Update();

            aiStateMachine.Update();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            aiStateMachine.OnTriggerEnter(other);
        }

        public Vector2 AdjustVectorAccuracy(Vector2 vector, float accuracy)
        {
            // Clamp accuracy to the range of 0 to 1 to avoid unexpected results
            accuracy = Mathf.Clamp01(accuracy);

            // Calculate the maximum deviation based on the accuracy
            float deviation = 1 - accuracy;

            // Generate a random unit vector (random direction)
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            // Generate a random magnitude based on the deviation
            // float randomMagnitude = Random.Range(0f, deviation);
            float randomMagnitude = Random.Range(0, (passRangeMax - passRangeMin) * deviation) + passRangeMin;

            // Calculate the random offset
            Vector2 randomOffset = randomDirection * randomMagnitude;

            // Add the random offset to the original vector
            return vector + randomOffset;
        }

        public Vector3 GetMyDefensivePosition(Vector3 defensivePos) {
            return new Vector3(defensivePos.x * courtSide, defensivePos.y, defensivePos.z * Mathf.Sign(defensivePos.z) == Mathf.Sign(teammate.Skills.Position.z) ? -1 : 1);
        }

        protected override void OnDrawGizmos() {
            base.OnDrawGizmos();
            // Dig Range
            if (skills != null) {
                GizmoHelpers.DrawGizmoCircle(transform.position, skills.DigRange, Color.red, 12);
            }
        }

        //---- PROPERTIES ----
        public StateMachine StateMachine { get { return aiStateMachine; } }
        public ServeState ServeState { get { return serveState; } }
        public DefenseState DefenseState { get { return defenseState; } }
        public OffenseState OffenseState { get { return offenseState; } }
        public DigReadyState DigReadyState { get { return digReadyState; } }
        public PostPointState PostPointState { get { return postPointState; } }
        public Athlete Teammate { get { return teammate; } set { teammate = value; } }
        public Vector3 DefensePos {
            get {
                float defenseZPos;
                if (teammate.GetComponent<Player>() != null || teammate.Skills.PlayerPosition == PositionType.Defender) {
                    defenseZPos = GetMyDefensivePosition(transform.position).z;
                } else {
                    defenseZPos = transform.position.z;
                }

                return new Vector3(skills.DefensePos.x * courtSide, 0.01f, defenseZPos);
            }
        }
        public Vector3 OffensePos { get { return new Vector3(skills.OffenseXPos, 0.01f, transform.position.z); } }
    }
}
