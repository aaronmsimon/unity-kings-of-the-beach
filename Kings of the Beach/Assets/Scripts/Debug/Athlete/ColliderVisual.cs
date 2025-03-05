using UnityEngine;

namespace KotB.Testing
{
    public class ColliderVisual : MonoBehaviour
    {
        [SerializeField] private SphereCollider sphereCollider;

        private void OnDrawGizmos() {
            if (sphereCollider.enabled) {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position + sphereCollider.center, sphereCollider.radius);
            }            
        }
    }
}
