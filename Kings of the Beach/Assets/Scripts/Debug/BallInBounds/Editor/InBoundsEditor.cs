using UnityEditor;
using UnityEngine;

namespace KotB.Testing
{
    [CustomEditor(typeof(BallInBounds))]
    public class InBoundsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            BallInBounds bib = target as BallInBounds;
            if (GUILayout.Button("Check if ball is in bounds"))
                bib.CheckInBounds();
        }
    }
}
