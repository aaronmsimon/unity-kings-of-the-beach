using System;
using UnityEngine;
using TMPro;
using System.Collections;

namespace KotB.Menus
{
    public enum UIGroupType {
        Folder,
        Skills
    }

    public class UIGroupSelect : MonoBehaviour
    {
        [Header("Navigation")]
        [SerializeField] private GameObject navPrev;
        [SerializeField] private GameObject navNext;

        [Header("Group Data")]
        [SerializeField] private string resourcesPath;
        [SerializeField] private UIGroupSelect parentFolder;
        [SerializeField] private UIGroupType groupType;

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
            switch (groupType) {
                case UIGroupType.Folder:
                    groupItems = folderList.GetFolderArray(folderPath);
                    break;
                case UIGroupType.Skills:
                    SkillsSO[] skills = Resources.LoadAll<SkillsSO>(folderPath);
                    groupItems = new string[skills.Length];
                    for(int i = 0; i < skills.Length; i++) {
                        groupItems[i] = skills[i].AthleteName;
                    }
                    break;
                default:
                    Debug.LogAssertion("No Group Type selected.");
                    break;
            }
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
