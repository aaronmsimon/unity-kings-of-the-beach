using UnityEditor;
using UnityEngine;

namespace KotB.Testing
{
    [CustomEditor(typeof(DrawWireSphereHelper))]
    public class DrawWireSphereEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            DrawWireSphereHelper dwsh = target as DrawWireSphereHelper;
            if (GUILayout.Button("Clear Sphere"))
                dwsh.Clear();
        }
    }
}
