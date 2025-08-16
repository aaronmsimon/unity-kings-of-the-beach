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

        public string MenuGroupName => menuGroupName;
        public string ResourcesPath => resourcesPath;
        public int SubfolderIndex => subfolderIndex;
        public int Index => index;
    }
}
