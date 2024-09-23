using UnityEngine;
using RoboRyanTron.Unite2017.Events;
using RoboRyanTron.Unite2017.Variables;

public enum BallState {
    Held,
    Bump,
    OnGround,
    Serve
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

    [Header("Variables")]
    [SerializeField] private FloatVariable servePower;

    private Transform ballTarget;
    private float athleteColliderWidth = .35f;
    private float ballHeldHeight = 1.09f;

    // Pass
    private float ballHeight;
    private float ballDuration;
    private Vector3 startPos;
    private float time;

    // Serve
    private float idealServeHeight = 3f;
    private float minServeHeight = 2f;
    private float maxServeHeight = 5f;
    private float minServeDistance = 5f;
    private float maxServeDistance = 18f;
    private Vector3 serveP1;

    private void Start() {
        ballSO.ballState = BallState.OnGround;
    }

    private void Update() {
        switch (ballSO.ballState) {
            case BallState.Held:
                transform.position = ballSO.ballHeldBy.transform.position + new Vector3(ballSO.ballHeldBy.CourtSide * -athleteColliderWidth, ballHeldHeight, 0);
                break;
            case BallState.Bump:
                BallArc();
                break;
            case BallState.OnGround:
                break;
            case BallState.Serve:
                ServePath();
                break;
            default:
                Debug.LogWarning("Ball State unhandled.");
                break;
        }

        ballSO.Position = transform.position;
        if (ballTarget != null) {
            if ((ballTarget.position.x > 0 && ballSO.Possession == -1) || (ballTarget.position.x < 0 && ballSO.Possession == 1)) {
                ballSO.HitsForTeam = 0;
                ballSO.lastPlayerToHit = null;
                Debug.Log("changed side");
            }        
            ballSO.Possession = ballTarget.position.x > 0 ? 1 : -1;
        }

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
        ballSO.ballState = BallState.Bump;
    }

    private void BallArc() {
        Vector3 targetPos = ballTarget.position;
        if (ballSO.ballState == BallState.Bump && transform.position != targetPos) {
            time += Time.deltaTime;
            float t = time / ballDuration;
            if (t > 1f) t = 1f;

            Vector3 apex = new Vector3((startPos.x + targetPos.x) / 2, ballHeight, (startPos.z + targetPos.z) / 2);

            transform.position = CalculateQuadraticBezierPoint(t, startPos, apex, targetPos);
        } else {
            ballSO.ballState = BallState.OnGround;
            ballHitGroundEvent.Raise();
            DestroyBallTarget();
        }
    }

    // will do separate serve for now, but need to refactor this into one ball path with passing (and later, spike)
    public void Serve(Vector3 aimPoint, float duration) {
        float heightP1 = aimPoint.y;
        heightP1 = Mathf.Clamp(heightP1, minServeHeight, maxServeHeight);

        // Calculate the parabolic scaling factor
        float normalizedHeight = (heightP1 - idealServeHeight) / (maxServeHeight - minServeHeight);
        float parabolicFactor = 1f - Mathf.Clamp01(normalizedHeight * normalizedHeight);

        // Calculate the new distance based on the parabolic relationship
        float possibleDistance = minServeDistance + (maxServeDistance - minServeDistance) * parabolicFactor;
        float serveDistance = possibleDistance * servePower.Value;

        // Calculate Target Position
        Vector3 direction = new Vector3(aimPoint.x, 0, aimPoint.z).normalized;
        Vector3 targetPos = new Vector3(transform.position.x, 0, transform.position.z) + direction * serveDistance;

        startPos = transform.position;
        time = 0f;
        DestroyBallTarget();
        ballTarget = Instantiate(targetPrefab, targetPos, Quaternion.identity);
        ballSO.Target = targetPos;
        targetSet.Raise();
        serveP1 = aimPoint;
        ballDuration = duration;
        ballSO.ballState = BallState.Serve;
    }

    private void ServePath() {
        Vector3 targetPos = ballTarget.position;
        if (ballSO.ballState == BallState.Serve && transform.position != targetPos) {
            time += Time.deltaTime;
            float t = time / ballDuration;
            if (t > 1f) t = 1f;

            transform.position = CalculateQuadraticBezierPoint(t, startPos, serveP1, targetPos);
        } else {
            ballSO.ballState = BallState.OnGround;
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
        ballSO.ballState = BallState.Held;
    }

    private Vector3 origPos;
    private void Awake() {
        origPos = transform.position;
    }
}
