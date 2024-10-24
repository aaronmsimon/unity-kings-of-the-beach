using UnityEngine;
using UnityEditor;
using KotB.Actors;

namespace KotB.Testing
{
    [CustomEditor(typeof(AthleteThoughts))]
    public class EventEditor : Editor
    {
        private Vector2 scrollPos;  // Scroll position for the scrollable text box

        public override void OnInspectorGUI()
        {
            GUI.enabled = Application.isPlaying;

            // Get a reference to the target script
            AthleteThoughts thoughts = (AthleteThoughts)target;

            // Draw the default inspector (in case there are other fields)
            DrawDefaultInspector();

            // Add some spacing
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"{thoughts.name}'s Thoughts", EditorStyles.boldLabel);

            // Create a scrollable text area for the queue
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(100)); // Set a height of 100 for the scroll box

            if (thoughts.itemsQueue != null && thoughts.itemsQueue.Count > 0)
            {
                foreach (var item in thoughts.itemsQueue)
                {
                    EditorGUILayout.LabelField(item);
                }
            }
            else
            {
                EditorGUILayout.LabelField("Queue is empty");
            }

            EditorGUILayout.EndScrollView();

            // Add a button to clear the queue
            if (GUILayout.Button("Clear Queue"))
            {
                // Call the ClearQueue method when the button is clicked
                thoughts.ClearQueue();
            }

            // ----- State Machine -----
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current State:", EditorStyles.boldLabel, GUILayout.Width(100));
            EditorGUILayout.LabelField($"{thoughts.CurrentState}", GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

            // Refresh the inspector when a change is made
            if (GUI.changed)
            {
                EditorUtility.SetDirty(thoughts);
            }
        }
    }
}
