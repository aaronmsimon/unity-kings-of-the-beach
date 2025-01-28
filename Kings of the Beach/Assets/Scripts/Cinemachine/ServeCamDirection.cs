using UnityEngine;
using Cinemachine;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Cinemachine
{
    public class ServeCamDirection : MonoBehaviour {
        [SerializeField] private FloatVariable serveDir;

        private CinemachineVirtualCamera cam;
        private Vector3 camRot;

        private void Awake() {
            cam = GetComponent<CinemachineVirtualCamera>();
            camRot = new Vector3(9.8f, 90f, 0f);
        }

        public void OnUpdateCameraPriorty() {
            cam.transform.rotation = Quaternion.Euler(camRot.x, camRot.y * -serveDir.Value, camRot.z);
            Vector3 serveOffset = new Vector3(3, 3, 0);
            CinemachineTransposer transposer = cam.GetCinemachineComponent<CinemachineTransposer>();
            if (transposer != null) {
                transposer.m_FollowOffset = new Vector3(serveOffset.x * serveDir.Value, serveOffset.y, serveOffset.z);
            }
        }

        public void SetServeCamDir(float dir) {
            serveDir.Value = dir;
        }
    }
}
