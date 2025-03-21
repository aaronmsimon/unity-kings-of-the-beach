using System;
using UnityEngine;

namespace KotB.Menus
{
    public enum UIGroupType {
        Folder,
        ScriptableObject
    }

    public class UIGroupSelect : UIMenuSelectable
    {
        [Header("Group Data")]
        [SerializeField] private string resourcesPath;
        [SerializeField] private UIGroupSelect parentFolder;
        [SerializeField] private UIGroupType groupType;
        
        public event Action SelectionChanged;

        private FolderList folderList = new FolderList();
        protected string[] groupItems;
        private int groupItemIndex = 0;

        private void Start() {
            if (parentFolder != null) return;

            LoadSelections();
        }

        private void OnEnable() {
            if (parentFolder == null) return;

            parentFolder.SelectionChanged += OnParentSelectionChanged;
        }

        private void OnDisable() {
            if (parentFolder == null) return;

            parentFolder.SelectionChanged -= OnParentSelectionChanged;
        }

        public virtual void LoadSelections() {
            string folderPath = resourcesPath + (parentFolder != null ? $"/{parentFolder.GetSelectedValue()}" : null);
            switch (groupType) {
                case UIGroupType.Folder:
                    groupItems = folderList.GetFolderArray(folderPath);
                    break;
                case UIGroupType.ScriptableObject:
                    ScriptableObject[] scriptableObjects = Resources.LoadAll<ScriptableObject>(folderPath);
                    groupItems = new string[scriptableObjects.Length];
                    for(int i = 0; i < scriptableObjects.Length; i++) {
                        groupItems[i] = scriptableObjects[i].name;
                    }
                    break;
                default:
                    Debug.LogAssertion("No Group Type selected.");
                    break;
            }

            UpdateDisplay();
            RaiseSelectionChangedEvent();
        }

        public string GetSelectedValue() {
            if (groupItems != null && groupItemIndex >= 0 && groupItemIndex < groupItems.Length) {
                return groupItems[groupItemIndex];
            } else {
                return null;
            }
        }

        public void IncrementItemIndex(int direction) {
            if (direction > 0) {
                if (groupItemIndex < groupItems.Length - 1) {
                    groupItemIndex++;
                } else {
                    groupItemIndex = 0;
                }
            } else {
                if (groupItemIndex > 0) {
                    groupItemIndex--;
                } else {
                    groupItemIndex = groupItems.Length - 1;
                }
            }
            UpdateDisplay();
            RaiseSelectionChangedEvent();
        }

        protected void UpdateDisplay() {
            if (groupItems.Length > 0) {
                _menuText.text = groupItems[groupItemIndex];
            }
        }

        protected void RaiseSelectionChangedEvent() {
            SelectionChanged?.Invoke();
        }

        private void OnParentSelectionChanged() {
            groupItemIndex = 0;
            LoadSelections();
        }
    }
}
