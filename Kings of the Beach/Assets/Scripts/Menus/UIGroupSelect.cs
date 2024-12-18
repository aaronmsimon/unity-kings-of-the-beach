using System;
using UnityEngine;
using TMPro;

namespace KotB.Menus
{
    public class UIGroupSelect : MonoBehaviour
    {
        [Header("Navigation")]
        [SerializeField] private GameObject navPrev;
        [SerializeField] private GameObject navNext;

        [Header("Group Data")]
        [SerializeField] private string resourcesPath;
        [SerializeField] private UIGroupSelect parentFolder;

        public event Action SelectionChanged;

        private FolderList folderList = new FolderList();
        private string[] groupItems;
        private int groupItemIndex = 0;

        private TMP_Text display;

        private void Awake() {
            display = GetComponent<TMP_Text>();
        }

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
            string folderPath = resourcesPath + (parentFolder != null ? $"/{parentFolder.Selected()}" : null);
            groupItems = folderList.GetFolderArray(folderPath);
            Debug.Log($"group items length from {folderPath}: {groupItems.Length}");
            display.text = groupItems[groupItemIndex];
            SelectionChanged?.Invoke();
        }

        public string Selected() {
            if (groupItems != null && groupItemIndex >= 0 && groupItemIndex < groupItems.Length) {
                return groupItems[groupItemIndex];
            } else {
                return null;
            }
        }

        private void OnParentSelectionChanged() {
            LoadSelections();
        }
    }
}
