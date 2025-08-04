using UnityEngine;

namespace MenuSystem
{
    public class MenuOptionsSO : ScriptableObject
    {
        [Header("Group Data")]
        [SerializeField] private string folderPath;
        
        private string[] groupItems;
        private int groupItemIndex = 0;
        
        public void LoadSelections() {
            ScriptableObject[] scriptableObjects = Resources.LoadAll<ScriptableObject>(folderPath);
            groupItems = new string[scriptableObjects.Length];
            for(int i = 0; i < scriptableObjects.Length; i++) {
                groupItems[i] = scriptableObjects[i].name;
            }
        }

        public string GetSelectedValue() {
            if (groupItems != null && groupItemIndex >= 0 && groupItemIndex < groupItems.Length) {
                return groupItems[groupItemIndex];
            } else {
                return null;
            }
        }
    }
}
