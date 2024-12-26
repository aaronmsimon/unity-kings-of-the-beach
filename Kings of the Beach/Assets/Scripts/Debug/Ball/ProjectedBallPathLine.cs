using UnityEngine;
using KotB.Items;

namespace KotB.Testing
{
    public class ProjectedBallPathLine : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float lineTime = 5;
        private Ball ball;

        private void Awake() {
            ball = GetComponent<Ball>();
        }

        private void OnEnable() {
            ball.BallInfo.TargetSet += OnTargetSet;
        }

        private void OnDisable() {
            ball.BallInfo.TargetSet -= OnTargetSet;
        }

        private void OnTargetSet() {
            Vector3 startPos = ball.BallInfo.StartPos;
            Vector3 targetPos = ball.BallInfo.TargetPos;
            Vector3 distance = targetPos - startPos;
            Color lineColor;
            if (Physics.Raycast(startPos, distance.normalized, distance.magnitude, layerMask)) {
                lineColor = Color.red;
            } else {
                lineColor = Color.green;
            }
            Debug.DrawLine(startPos, targetPos, lineColor, lineTime);
        }
    }
}
