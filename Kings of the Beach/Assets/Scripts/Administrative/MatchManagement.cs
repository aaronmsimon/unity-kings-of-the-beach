using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public enum MatchState {
    Serve,
    BallInPlay,
    DeadBall
}

public class MatchManagement : MonoBehaviour
{
    [Header("Match Data Scriptable Object")]
    [SerializeField] private MatchSO matchData;

    [Header("Game Events")]
    [SerializeField] private GameEvent GameStateChangedEvent;

    private void SetMatchState(MatchState newState) {
        if (newState == matchData.State) return;

        matchData.State = newState;

        GameStateChangedEvent.Raise();
    }
}
