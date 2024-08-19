using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float bumpPower;
    [SerializeField] private Vector3 bumpDirection;

    private Vector3 startPos;

    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Bump(bumpPower);
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            ResetBall();
        }
    }

    public void Bump(float impulse) {
        rb.isKinematic = false;
        rb.AddForce(bumpDirection.normalized * impulse, ForceMode.Impulse);
    }

    private void ResetBall() {
        rb.isKinematic = true;
        transform.position = startPos;
    }
}
