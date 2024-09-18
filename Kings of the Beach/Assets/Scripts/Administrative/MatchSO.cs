using UnityEngine;

[CreateAssetMenu(fileName = "Match", menuName = "Game/Match Data")]
public class MatchSO : ScriptableObject
{
    [Header("Game State")]
    [SerializeField] private MatchState matchState;

    [Header("Service")]
    [SerializeField] private SkillsSO servicePlayer;

    public SkillsSO ServicePlayer {
        get {
            return servicePlayer;
        }
        set {
            servicePlayer = value;
        }
    }
    public MatchState State {
        get {
            return matchState;
        }
        set {
            matchState = value;
        }
    }
}
