using UnityEngine;

namespace KotB.Menus
{
    public class Orbit : MonoBehaviour
    {
        public Transform pivot;              // assign an object at the center
        public float degreesPerSecond = 20f; // orbit speed
        public Vector3 axis = Vector3.up;    // orbit axis (Y = horizontal circle)
        public bool lookAtPivot = true;      // keep camera facing the center
        public bool useUnscaledTime = true;  // works even if timeScale == 0

        void Update()
        {
            if (!pivot) return;

            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            transform.RotateAround(pivot.position, axis, degreesPerSecond * dt);

            if (lookAtPivot)
                transform.LookAt(pivot.position, Vector3.up);
        }
    }
}
