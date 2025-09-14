using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class ShowTargetPos : MonoBehaviour
    {
        [Header("Target Area")]
        [SerializeField] private Vector2 targetZoneSize = new Vector2(0.5f, 0.5f);
        [SerializeField] private bool showTargetZone = true;
        [SerializeField] private Color targetZoneColor = Color.blue;

        private AI ai;

        private void Start() {
            ai = GetComponent<AI>();
            if (ai == null) Debug.LogAssertion("No Athlete found.");
        }

        protected void OnDrawGizmos() {
            if (ai != null)
                Helpers.DrawTargetZone(new Vector2(ai.TargetPos.x, ai.TargetPos.z), targetZoneSize, targetZoneColor, showTargetZone);
        }
    }
}
