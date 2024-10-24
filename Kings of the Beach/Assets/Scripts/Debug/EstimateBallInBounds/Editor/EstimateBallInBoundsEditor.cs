using UnityEditor;
using UnityEngine;

namespace KotB.Testing
{
    [CustomEditor(typeof(EstimateBallInBounds))]
    public class EstimateBallInBoundsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            EstimateBallInBounds ebib = target as EstimateBallInBounds;
            if (GUILayout.Button("Check judgement"))
                ebib.CheckJudgement();
        }
    }
}
