using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class AIFollowGameObject : MonoBehaviour
    {
        [SerializeField] private AI ai;

        private void Update() {
            ai.TargetPos = transform.position;
        }
    }
}
