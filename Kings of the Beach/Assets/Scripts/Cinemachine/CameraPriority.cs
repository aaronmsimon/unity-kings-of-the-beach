using UnityEngine;
using Cinemachine;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Cinemachine
{
    public class CameraPriority : MonoBehaviour {
        [SerializeField] private FloatVariable camPriority;

        private CinemachineVirtualCamera cam;

        private void Awake() {
            cam = GetComponent<CinemachineVirtualCamera>();
        }

        public void OnUpdateCameraPriorty() {
            cam.Priority = (int)camPriority.Value;
        }
    }
}
