using System;
using System.Collections.Generic;
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
        private string[] groupItems;
        private int groupItemIndex = 0;
        private bool useManualList;
        [SerializeField] private List<string> manualSelectionsList;

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

        public void LoadSelections() {
            if (!useManualList) {
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
            } else {
                groupItems = manualSelectionsList.ToArray();
            }

            UpdateDisplay();
            SelectionChanged?.Invoke();
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
            SelectionChanged?.Invoke();
        }

        private void UpdateDisplay() {
            if (groupItems.Length > 0) {
                _menuText.text = groupItems[groupItemIndex];
            }
        }

        private void OnParentSelectionChanged() {
            LoadSelections();
        }

        // --- PROPERTIES ---
        public bool UseManualList { get { return useManualList; } set { useManualList = value; } }
        public List<string> ManualSelectionsList { get { return manualSelectionsList; } set { manualSelectionsList = value; } }
    }
}
