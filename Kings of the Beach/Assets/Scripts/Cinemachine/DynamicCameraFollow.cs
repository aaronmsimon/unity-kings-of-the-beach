using UnityEngine;
using Cinemachine;

namespace KotB.Cinemachine
{
    public class DynamicCameraFollow : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private Transform ball;
        
        [Header("Settings")]
        [SerializeField] private Vector3 mainPos; // The default camera position
        [SerializeField] private float visibleXRange = 8f; // How far left/right the ball can go before the camera moves
        [SerializeField] private float visibleZRange = 5f; // How far forward the ball can go before the camera moves back

        [Header("Behavior")]
        [SerializeField] private float returnSpeed = 2f; // Speed at which the camera returns to mainPos

        private Vector3 targetPos;

        void Start()
        {
            targetPos = mainPos; // Start at main position
        }

        void LateUpdate()
        {
            if (ball == null) return;

            Vector3 ballPos = ball.position;
            targetPos = mainPos;

            // Check X boundary
            if (Mathf.Abs(ballPos.x - mainPos.x) > visibleXRange)
            {
                targetPos.x = ballPos.x - Mathf.Sign(ballPos.x - mainPos.x) * visibleXRange;
            }

            // Check Z boundary
            if (ballPos.z < visibleZRange)
            {
                targetPos.z = mainPos.z - (visibleZRange - ballPos.z);
            }

            // Smoothly move the camera to the target position
            transform.position = Vector3.Lerp(transform.position, targetPos, returnSpeed * Time.deltaTime);
        }
    }
}
