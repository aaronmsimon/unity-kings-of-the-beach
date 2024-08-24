using UnityEngine;
using System.Collections;
using RoboRyanTron.Unite2017.Events;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform targetPrefab;
    [SerializeField] GameEvent targetDestroyedEvent;
    [SerializeField] GameEvent resetBallEvent;

    private Transform ballTarget;

    public void Bump(Vector3 targetPos, float height, float duration) {
        ballTarget = Instantiate(targetPrefab, targetPos, Quaternion.identity);
        StartCoroutine(BallArc(targetPos, height, duration));
    }

    private IEnumerator BallArc(Vector3 targetPos, float height, float duration) {
        Vector3 startPos = transform.position;
        float time = 0f;

        while(transform.position != targetPos) {
            time += Time.deltaTime;
            float t = time / duration;
            if (t > 1f) t = 1f;

            Vector3 apex = new Vector3((startPos.x + targetPos.x) / 2, height, (startPos.z + targetPos.z) / 2);

            transform.position = CalculateQuadraticBezierPoint(t, startPos, apex, targetPos);

            yield return null;
        }

        Destroy(ballTarget.gameObject);
        targetDestroyedEvent.Raise();
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

    public void ResetBall(/*Vector3 ballResetPos*/) {
        // transform.position = ballResetPos;
        transform.position = origPos;
    }

    /* temp */
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            resetBallEvent.Raise();
        }
    }
    private Vector3 origPos;
    private void Awake() {
        origPos = transform.position;
    }
}
