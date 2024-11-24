using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class SetAITarget : MonoBehaviour
    {
        [SerializeField] private Vector3 targetPos = new Vector3(0, 0.01f, 0);

        private AI ai;

        private void Awake() {
            ai = GetComponent<AI>();
        }

        private void Start() {
            targetPos = ai.transform.position;
        }

        private void Update() {
            ai.TargetPos = targetPos;
        }
    }
}
