using UnityEngine;
using UnityEditor;

namespace KotB.Menus
{
    [CustomEditor(typeof(UIGroupSelect))]
    public class UIGroupSelectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Add space at the top
            GUILayout.Space(10); // Adjust the value to increase or decrease the space

            UIGroupSelect uiGS = target as UIGroupSelect;

            uiGS.UseManualList = EditorGUILayout.Toggle("Use Manual List", uiGS.UseManualList);

            if (uiGS.UseManualList) {
                EditorGUILayout.LabelField("Manual List for Menu Selection", EditorStyles.boldLabel);

                // Display the list in a foldout to keep it clean
                if (uiGS.ManualSelectionsList.Count > 0) {
                    for (int i = 0; i < uiGS.ManualSelectionsList.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        
                        // Display and allow editing of each string
                        uiGS.ManualSelectionsList[i] = EditorGUILayout.TextField($"Manual Item {i}", uiGS.ManualSelectionsList[i]);

                        // Button to remove the string
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            uiGS.ManualSelectionsList.RemoveAt(i);
                            break;
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                }

                // Button to add new strings to the list
                if (GUILayout.Button("Add String"))
                {
                    uiGS.ManualSelectionsList.Add(string.Empty);
                }
            }

            if (GUI.changed)
            {
                Debug.Log("gui changed and am setting it as dirty so it saves");
                EditorUtility.SetDirty(target);
            }
        }
    }
}
