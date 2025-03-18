using UnityEditor;
using UnityEngine;

namespace KotB.Testing
{
    [CustomEditor(typeof(SetAthleteState))]
    public class SetAthleteStateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            SetAthleteState setState = target as SetAthleteState;
            if (GUILayout.Button("Set State"))
                setState.SetState();
        }
    }
}
