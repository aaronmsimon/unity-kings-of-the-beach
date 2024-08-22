using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public void Bump(Vector3 targetPos, float height, float duration) {
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

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ResetBall();
        }
    }

    private Vector3 origPos;
    private void Awake() {
        origPos = transform.position;
    }
    private void ResetBall() {
        transform.position = origPos;
    }
}
