using System;
using KotB.Actors;
using UnityEngine;

[CreateAssetMenu(fileName = "BallInfo", menuName = "Game/Ball Info")]
public class BallSO : ScriptableObject
{
    public Vector3 Position { get; set; }
    public Vector3 TargetPos { get; set; }
    public Vector3 StartPos { get; private set; }
    public float Height { get; set; }
    public float Duration { get; set; }
    public int Possession { get; set; }
    public int HitsForTeam { get; set; }
    public Athlete lastPlayerToHit { get; set; }
    public float TimeSinceLastHit { get; set; }

    public Athlete ballHeldBy { get; private set; }

    public event Action BallGiven;
    public event Action TargetSet;

    // Serve
    private float idealServeHeight = 3f;
    private float minServeHeight = 2f;
    private float maxServeHeight = 5f;
    private float minServeDistance = 7f;
    private float maxServeDistance = 18f;
    
    public void GiveBall(Athlete athlete) {
        ballHeldBy = athlete;
        BallGiven?.Invoke();
    }

    public void SetServeTarget(Vector3 aimPoint, float servePower) {
        Height = aimPoint.y;
        Height = Mathf.Clamp(Height, minServeHeight, maxServeHeight);

        // Calculate the parabolic scaling factor
        float normalizedHeight = (Height - idealServeHeight) / (maxServeHeight - minServeHeight);
        float parabolicFactor = 1f - Mathf.Clamp01(normalizedHeight * normalizedHeight);

        // Calculate the new distance based on the parabolic relationship
        float possibleDistance = minServeDistance + (maxServeDistance - minServeDistance) * parabolicFactor;
        float serveDistance = possibleDistance * servePower;

        // Calculate Target Position
        Vector3 direction = new Vector3(aimPoint.x, 0, aimPoint.z) - new Vector3(Position.x, 0, Position.z);
        direction.Normalize();
        TargetPos = new Vector3(Position.x, 0, Position.z) + direction * serveDistance;

        StartPos = Position;
        Duration = 1.5f; // this needs to be based on power and distance
        ResetTimeSinceLastHit();
        TargetSet?.Invoke();
    }

    public void SetPassTarget(Vector3 targetPos, float height, float duration) {
        StartPos = Position;
        TargetPos = targetPos;
        Height = height;
        Duration = duration;
        ResetTimeSinceLastHit();
        TargetSet?.Invoke();
    }

    private void ResetTimeSinceLastHit() {
        TimeSinceLastHit = 0;
    }
}
