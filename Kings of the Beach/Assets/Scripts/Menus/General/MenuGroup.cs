using System;
using UnityEngine;

namespace KotB.Menus
{
    [Serializable]
    public class MenuGroup
    {
        [SerializeField] private string menuGroupName;
        [SerializeField] private string resourcesPath;
        [SerializeField] private int subfolderIndex;

        public string Text => options[index];

        public event Action SelectionChanged;

        private string[] options;
        private int index = 0;

        public void LoadOptions(string subfolder = null) {
            string folderPath = resourcesPath + (subfolder != null ? $"/{subfolder}" : null);
            ScriptableObject[] scriptableObjects = Resources.LoadAll<ScriptableObject>(folderPath);
            options = new string[scriptableObjects.Length];

            for(int i = 0; i < scriptableObjects.Length; i++) {
                IMenuSelection menuSelection = (IMenuSelection)scriptableObjects[i];
                options[i] = menuSelection.GetMenuText();
            }

            SelectionChanged?.Invoke();
        }

        public void IncrementItemIndex(int direction) {
            if (direction > 0) {
                if (index < options.Length - 1) {
                    index++;
                } else {
                    index = 0;
                }
            } else {
                if (index > 0) {
                    index--;
                } else {
                    index = options.Length - 1;
                }
            }

            SelectionChanged?.Invoke();
        }

        public void SetItemIndex(int index) {
            this.index = index;
        }

        public int GetIndexByName(string name) {
            // Returns -1 if not found
            return Array.FindIndex(options, o => o == name);
        }

        public string MenuGroupName => menuGroupName;
        public string ResourcesPath => resourcesPath;
        public int SubfolderIndex => subfolderIndex;
        public int Index => index;
    }
}
