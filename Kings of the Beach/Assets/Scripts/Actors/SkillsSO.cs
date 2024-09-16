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

    [Header("Range")]
    [SerializeField] private float digRange;

    [Header("Serving")]
    [SerializeField][Range(1,10)] private int serving;

    [Header("Behavior")]
    [SerializeField] private float myBlockArea = 1.5f;

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
    public float DigRange {
        get {
            return digRange;
        }
    }
    public float MyBlockArea {
        get {
            return myBlockArea;
        }
    }
    public int Serving {
        get {
            return serving;
        }
    }
}
