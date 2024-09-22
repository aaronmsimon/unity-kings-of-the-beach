using UnityEngine;
using Cinemachine;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Cinemachine
{
    public class CinemachinePriority : MonoBehaviour {
        [SerializeField] private FloatVariable cameraPriority;

        private CinemachineVirtualCamera cam;

        private void Awake() {
            cam = GetComponent<CinemachineVirtualCamera>();
        }

        public void OnUpdateCameraPriorty() {
            cam.Priority = (int)cameraPriority.Value;
        }
    }
}
