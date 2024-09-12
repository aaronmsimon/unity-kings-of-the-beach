using UnityEngine;

public enum PositionType {
    Blocker,
    Defender
}

[CreateAssetMenu(fileName = "Skills", menuName = "Game/Actor")]
public class SkillsSO : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float targetLockDistance;

    [Header("Accuracy")]
    [SerializeField][Range(0,1)] private float passAccuracy;

    [Header("Position")]
    [SerializeField] private PositionType playerPosition;

    private Vector3 position;

    public float MoveSpeed {
        get {
            return moveSpeed;
        }
    }
    public float TargetLockDistance {
        get {
            return targetLockDistance;
        }
    }
    public float PassAccuracy {
        get {
            return passAccuracy;
        }
    }
    public Vector3 Position {
        get {
            return position;
        }
        set {
            position = value;
        }
    }
    public PositionType PlayerPosition {
        get {
            return playerPosition;
        }
    }
}
