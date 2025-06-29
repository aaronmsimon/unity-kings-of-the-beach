using System.Collections.Generic;
using UnityEngine;
using KotB.Items;

namespace KotB.Testing
{
    public class BallPath : MonoBehaviour
    {
        [SerializeField] private GameObject ballPathMarker;
        [SerializeField] private float frequency = 4;
        [SerializeField] private bool logPositions = false;

        private Ball ball;
        private bool trackingBallPath;
        private float hz;
        private float timeSinceLastMarker;
        private List<GameObject> pathMarkers = new List<GameObject>();
        private Vector3 lastPos;

        private void Awake() {
            ball = GetComponent<Ball>();
            trackingBallPath = false;
        }

        private void OnEnable() {
            ball.BallInfo.TargetSet += OnTargetSet;
        }

        private void OnDisable() {
            ball.BallInfo.TargetSet -= OnTargetSet;
        }

        private void Update() {
            timeSinceLastMarker -= Time.deltaTime;

            if (trackingBallPath && timeSinceLastMarker < 0) {
                GameObject pathMarker = Instantiate(ballPathMarker, transform.position, Quaternion.identity);
                pathMarkers.Add(pathMarker);
                ResetTimeSinceLastMarker();
                if (logPositions && transform.position != lastPos) Debug.Log($"Ball is at {transform.position}");
            }

            lastPos = transform.position;
        }

        private void OnTargetSet() {
            trackingBallPath = true;
            hz = 1 / frequency;
            ResetTimeSinceLastMarker();
            DestroyAllPathMarkers();
        }

        public void OnBallHitGround() {
            trackingBallPath = false;
        }

        private void ResetTimeSinceLastMarker() {
            timeSinceLastMarker = hz;
        }

        public void DestroyAllPathMarkers() {
            foreach (GameObject marker in pathMarkers) {
                Destroy(marker);
            }
            pathMarkers.Clear();
        }
    }
}
