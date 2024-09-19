using UnityEngine;
using KotB.StateMachine;

public class MatchManager : MonoBehaviour
{
    [SerializeField] private MatchStateSO matchStateSO;
    [SerializeField] private InputReader inputReader;

    private MatchStateMachine matchStateMachine;
    private PrePointState prePointState;
    private ServeState serveState;

    public MatchStateMachine MatchStateMachine { get { return matchStateMachine; } }
    // public PrePointState PrePointState { get; }
    public ServeState ServeState { get { return serveState; } }
    public InputReader InputReader { get { return inputReader; } }

    private void Start() {
        matchStateMachine = new MatchStateMachine();
        prePointState = new PrePointState(this);
        serveState = new ServeState(this);

        matchStateMachine.StateChanged += OnStateChanged;

        matchStateMachine.ChangeState(prePointState);
    }

    private void OnStateChanged(State newState) {
        matchStateSO.CurrentState = newState;
    }
}
