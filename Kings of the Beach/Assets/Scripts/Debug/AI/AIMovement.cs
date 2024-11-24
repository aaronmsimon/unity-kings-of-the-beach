using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class AIMovement : MonoBehaviour
    {
        [SerializeField] private AI aiAthlete;

        private Vector3 moveDestination;
        private float destBuffer = 0f;

        private void Update() {
            Vector3 mousePosition = GetMousePosition();

            if (Input.GetMouseButtonDown(0)) {
                moveDestination = mousePosition;
            }

            if (moveDestination != Vector3.down && Vector3.Distance(moveDestination, aiAthlete.transform.position) > destBuffer) {
                aiAthlete.MoveDir = moveDestination - aiAthlete.transform.position;
            }
        }

        public Vector3 GetMousePosition() {
            // Set a ray from the screen to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Don't need to fetch the ground plane from the game world, but just create it programmatically
            Plane groundPlane = new Plane(Vector3.up, 0.01f);
            float rayDistance;

            if(groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 pointOfIntersection = ray.GetPoint(rayDistance);
                Debug.DrawLine(ray.origin, pointOfIntersection, Color.red);

                return pointOfIntersection;
            } else {
                return Vector3.down;
            }
        }
    }
}
