using UnityEngine;

namespace KotB.Testing
{
    public class RotationDebugger : MonoBehaviour 
    {
        private Quaternion lastRotation;

        void Start()
        {
            lastRotation = transform.rotation;
        }

        float GetSmallestAngleDifference(float angle1, float angle2)
        {
            // Get the raw difference
            float difference = Mathf.Abs(angle1 - angle2);
            // Return the smaller of the direct difference or the wrapped difference
            return Mathf.Min(difference, 360 - difference);
        }

        void LateUpdate()
        {
            float xAngleDiff = GetSmallestAngleDifference(
                lastRotation.eulerAngles.x, 
                transform.rotation.eulerAngles.x
            );

            if (xAngleDiff > 50f)
            {
                Debug.LogFormat("Large X rotation detected! From {0} to {1} (diff: {2})", 
                    lastRotation.eulerAngles, 
                    transform.rotation.eulerAngles,
                    xAngleDiff);
                Debug.Log($"Call stack at time of rotation change:\n{new System.Diagnostics.StackTrace(true)}");
            }
            lastRotation = transform.rotation;
        }
    }
}
