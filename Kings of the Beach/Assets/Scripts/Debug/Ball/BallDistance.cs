using UnityEngine;
using KotB.Items;

namespace KotB.Testing
{
    public class BallDistance : MonoBehaviour
    {
        [SerializeField] BallSO ballInfo;

        private void OnEnable() {
            ballInfo.TargetSet += OnTargetSet;
        }

        private void OnDisable() {
            ballInfo.TargetSet -= OnTargetSet;
        }

        private void OnTargetSet() {
            Vector2 startPos = new Vector2(ballInfo.Position.x, ballInfo.Position.z);
            Vector2 targetPos = new Vector2(ballInfo.TargetPos.x, ballInfo.TargetPos.z);
            Debug.Log($"Target Set from {startPos} to {targetPos} for a distance of {Vector3.Distance(targetPos, startPos)}");
        }
    }
}
