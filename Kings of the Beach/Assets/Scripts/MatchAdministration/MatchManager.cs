using UnityEngine;
using KotB.StateMachine;
using RoboRyanTron.Unite2017.Events;

public class MatchManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private MatchStateSO matchStateSO;
    [SerializeField] private InputReader inputReader;

    [Header("Events")]
    [SerializeField] private GameEvent changeToServeState;
    [SerializeField] private GameEvent showPowerMeter;

    private MatchStateMachine matchStateMachine;
    private PrePointState prePointState;
    private ServeState serveState;

    private void Start() {
        matchStateMachine = new MatchStateMachine();
        prePointState = new PrePointState(inputReader, changeToServeState);
        serveState = new ServeState(inputReader, showPowerMeter);

        matchStateMachine.StateChanged += OnStateChanged;

        matchStateMachine.ChangeState(prePointState);
    }

    public void OnChangeToServeState() {
        matchStateMachine.ChangeState(serveState);
    }

    private void OnStateChanged(State newState) {
        matchStateSO.CurrentState = newState;
    }
}
