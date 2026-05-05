using UnityEditor;
using UnityEngine;

namespace KotB.Testing
{
    [CustomEditor(typeof(InstantReplay))]
    public class InstantReplayEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            InstantReplay ir = target as InstantReplay;
            if (GUILayout.Button("Run Instant Replay"))
                ir.RunInstantReplay();
        }
    }
}
