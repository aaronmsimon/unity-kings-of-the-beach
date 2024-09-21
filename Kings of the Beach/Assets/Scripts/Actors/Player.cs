using UnityEngine;
using KotB.StatePattern;
using KotB.StatePattern.PlayerStates;

namespace KotB.Actors
{
    public class Player : Athlete
    {
        [Header("Player Controls")]
        [SerializeField] private float coyoteTime;
        [SerializeField] private InputReader inputReader;

        private Vector3 moveInput;

        private StateMachine playerStateMachine;
        private NormalState normalState;
        private LockState lockState;
        private ServeState serveState;
        private PostPointState postPointState;

        protected override void Start() {
            base.Start();

            playerStateMachine = new StateMachine();
            normalState = new NormalState(this);
            lockState = new LockState(this);
            serveState = new ServeState(this);
            postPointState = new PostPointState(this);
        }

        //Adds listeners for events being triggered in the InputReader script
        private void OnEnable() {
            inputReader.moveEvent += OnMove;
        }
        
        //Removes all listeners to the events coming from the InputReader script
        private void OnDisable() {
            inputReader.moveEvent -= OnMove;
        }
        
        private Vector2 CircleMappedToSquare(float u, float v) {
            float u2 = u * u;
            float v2 = v * v;
            float tworoot2 = 2 * Mathf.Sqrt(2);
            float subtermX = 2 + u2 - v2;
            float subtermY = 2 - u2 + v2;
            float termX1 = subtermX + u * tworoot2;
            float termX2 = subtermX - u * tworoot2;
            float termY1 = subtermY + v * tworoot2;
            float termY2 = subtermY - v * tworoot2;

            float epsilon = 0.0000001f;
            if (termX1 < epsilon)
                termX1 = 0;
            if (termX2 < epsilon)
                termX2 = 0;
            if (termY1 < epsilon)
                termY1 = 0;
            if (termY2 < epsilon)
                termY2 = 0;

            float x = Mathf.Clamp(0.5f * Mathf.Sqrt(termX1) - 0.5f * Mathf.Sqrt(termX2), -1, 1);
            float y = Mathf.Clamp(0.5f * Mathf.Sqrt(termY1) - 0.5f * Mathf.Sqrt(termY2), -1, 1);

            return new Vector2(x, y);
        }

        //---- EVENT LISTENERS ----

        private void OnMove(Vector2 movement) {
            moveInput = movement;
            moveDir = new Vector3(movement.x, 0, movement.y);
        }

        //---- PROPERTIES ----

        public InputReader InputReader { get { return inputReader; } }
        public StateMachine StateMachine { get { return StateMachine; } }
        public LockState LockState { get { return lockState; } }
        public ServeState ServeState { get { return serveState; } }
        public PostPointState PostPointState { get { return postPointState; } }
    }
}
