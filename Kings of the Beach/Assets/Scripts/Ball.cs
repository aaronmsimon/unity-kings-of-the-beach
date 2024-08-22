using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public void Bump(float bumpPower, Vector3 targetPos, float ballSpeed) {
        StartCoroutine(BallArc(bumpPower, targetPos, ballSpeed));
    }

    private IEnumerator BallArc(float bumpPower, Vector3 targetPos, float ballSpeed) {
        Vector3 startPos = transform.position;
        float percent = 0f;

        while(transform.position.y >= 0) {
            percent += Time.deltaTime * ballSpeed;
            float x = Mathf.Lerp(startPos.x, targetPos.x, percent);
            float y = bumpPower * (-percent * percent + percent) + startPos.y;
            float z = Mathf.Lerp(startPos.z, targetPos.z, percent);
            
            transform.position = new Vector3(x, y, z);

            yield return null;
        }
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
