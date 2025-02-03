using UnityEngine;
using Cinemachine;
using KotB.Items;
using KotB.Match;

namespace KotB.Cinemachine
{
    public class MainCamController : MonoBehaviour
    {
        [SerializeField] private Vector3 camStartPos;
        [SerializeField] private Ball ball;
        [SerializeField] private MatchInfoSO matchInfo;

        private CinemachineVirtualCamera cam;

        private void Awake() {
            cam = GetComponent<CinemachineVirtualCamera>();
        }

        private void OnEnable() {
            matchInfo.TransitionToServeState += OnTransitionToServeState;
            matchInfo.TransitionToInPlayState += OnTransitionToInPlayState;
        }

        private void OnDisable() {
            matchInfo.TransitionToServeState -= OnTransitionToServeState;
            matchInfo.TransitionToInPlayState -= OnTransitionToInPlayState;
        }

        public void OnTransitionToServeState() {
            Debug.Log("serve state event");
            cam.transform.position = camStartPos;
            cam.Follow = null;
        }

        public void OnTransitionToInPlayState() {
            Debug.Log("in play state event");
            if (ball != null) {
                cam.Follow = ball.transform;
            }
        }
    }
}
