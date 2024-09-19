using UnityEngine;
using KotB.StateMachine;

public class MatchManager : MonoBehaviour
{
    [SerializeField] private MatchStateSO matchStateSO;

    private MatchStateMachine matchStateMachine;
    private PrePointState prePointState;

    private void Start() {
        matchStateMachine = new MatchStateMachine();
        prePointState = new PrePointState();

        matchStateMachine.StateChanged += OnStateChanged;

        matchStateMachine.ChangeState(prePointState);
    }

    private void OnStateChanged(State newState) {
        matchStateSO.CurrentState = newState;
    }

    // temp
    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) {
            
        }
    }
}
