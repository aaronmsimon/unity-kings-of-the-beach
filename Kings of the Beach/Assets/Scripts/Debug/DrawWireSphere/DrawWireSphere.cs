using UnityEngine;

namespace KotB.Testing
{
    public class DrawWireSphereHelper : MonoBehaviour
    {
        [SerializeField] private Vector3 posOffset;
        [SerializeField] private float sphereRadius = 1f;
        [SerializeField] private Color sphereColor = Color.yellow;
        
        private bool showSphere = false;
        private Vector3 spherePos;
        
        public void Draw()
        {
            spherePos = transform.position + posOffset;
            showSphere = true;
        }

        public void Clear() {
            showSphere = false;
        }
        
        private void OnDrawGizmos()
        {
            // Only draw when the sphere should be shown
            if (showSphere)
            {
                Gizmos.color = sphereColor;
                Gizmos.DrawWireSphere(spherePos, sphereRadius);
            }
        }
    }
}
