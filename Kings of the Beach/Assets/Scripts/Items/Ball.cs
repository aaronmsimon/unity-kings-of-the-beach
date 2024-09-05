using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public enum BallState {
    Held,
    Bump,
    OnGround
}

public class Ball : MonoBehaviour
{
    [SerializeField] private BallSO ballSO;
    [Space(10)]

    [SerializeField] private Transform targetPrefab;
    
    [Header("Game Events")]
    [SerializeField] private GameEvent resetBallEvent;
    [SerializeField] private GameEvent targetSet;
    [SerializeField] private GameEvent ballHitGroundEvent;

    private Transform ballTarget;
    private BallState ballState;
    private float ballHeight;
    private float ballDuration;
    private Vector3 startPos;
    private float time;

    private void Start() {
        ballState = BallState.Held;
    }

    private void Update() {
        switch (ballState) {
            case BallState.Held:
                break;
            case BallState.Bump:
                BallArc();
                break;
            case BallState.OnGround:
                break;
            default:
                Debug.LogWarning("Ball State unhandled.");
                break;
        }

        ballSO.Position = transform.position;
        ballSO.ballState = ballState;
        if ((transform.position.x > 0 && ballSO.Possession == -1) || (transform.position.x < 0 && ballSO.Possession == 1)) {
            ballSO.HitsForTeam = 0;
            ballSO.lastPlayerToHit = null;
            Debug.Log("changed side");
        }        
        ballSO.Possession = transform.position.x > 0 ? 1 : -1;

        /* temp */
        if (Input.GetKeyDown(KeyCode.Space)) {
            resetBallEvent.Raise();
        }
    }
    
    public void Bump(Vector3 targetPos, float height, float duration) {
        startPos = transform.position;
        time = 0f;
        DestroyBallTarget();
        ballTarget = Instantiate(targetPrefab, targetPos, Quaternion.identity);
        ballSO.Target = targetPos;
        targetSet.Raise();
        ballHeight = height;
        ballDuration = duration;
        ballState = BallState.Bump;
    }

    private void BallArc() {
        Vector3 targetPos = ballTarget.position;
        if (ballState == BallState.Bump && transform.position != targetPos) {
            time += Time.deltaTime;
            float t = time / ballDuration;
            if (t > 1f) t = 1f;

            Vector3 apex = new Vector3((startPos.x + targetPos.x) / 2, ballHeight, (startPos.z + targetPos.z) / 2);

            transform.position = CalculateQuadraticBezierPoint(t, startPos, apex, targetPos);
        } else {
            ballState = BallState.OnGround;
            ballHitGroundEvent.Raise();
            DestroyBallTarget();
        }
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    private void DestroyBallTarget() {
        if (ballTarget != null) {
            Destroy(ballTarget.gameObject);
        }
    }

    public void ResetBall(/*Vector3 ballResetPos*/) {
        // transform.position = ballResetPos;
        DestroyBallTarget();
        transform.position = origPos;
        ballState = BallState.Held;
    }

    private Vector3 origPos;
    private void Awake() {
        origPos = transform.position;
    }
}
