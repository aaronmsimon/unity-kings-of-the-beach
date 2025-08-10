using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Drives one or more UI Toolkit ListViews from Resources folders,
/// with optional parent → child dependency:
/// - Root group: loads from BasePath
/// - Child group: loads from BasePath + "/" + ParentSelectedKey
/// When a parent's selection changes, children auto‑reload and cascade.
/// </summary>
namespace KotB.Menus
{
    public class ListViewMenuController : MonoBehaviour
    {
        [Serializable]
        public class GroupConfig
        {
            [Tooltip("UXML name of the ListView (root.Q<ListView>(this)).")]
            public string listViewName;

            [Tooltip("Base Resources path for this group, e.g. \"Athletes/Olympics 2024/Male\".")]
            public string baseResourcesPath;

            [Tooltip("Index of the parent group in this array (-1 if none).")]
            public int dependsOnIndex = -1;

            [Tooltip("Sort items alphabetically (case-insensitive).")]
            public bool sortAlphabetically = true;

            [Tooltip("Auto-select index 0 when this group reloads or becomes non-empty.")]
            public bool selectFirstOnReload = true;
        }

        [Header("UI & Groups")]
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private List<GroupConfig> groups = new();

        private readonly List<ListView> _listViews = new();
        private readonly List<List<string>> _displayNames = new();
        private readonly List<List<string>> _keys = new();
        private Dictionary<int, List<int>> _childrenOf;
        
        private void Awake()
        {
            if (uiDocument == null) uiDocument = GetComponent<UIDocument>();
            if (uiDocument == null || uiDocument.rootVisualElement == null)
            {
                Debug.LogError("ListViewMenuController: UIDocument (or root) is missing.");
                enabled = false;
                return;
            }

            var root = uiDocument.rootVisualElement;
            _listViews.Clear();
            _displayNames.Clear();
            _keys.Clear();

            // Initialize each group’s ListView and data lists
            for (int i = 0; i < groups.Count; i++)
            {
                var cfg = groups[i];
                var lv = root.Q<ListView>(cfg.listViewName);
                if (lv == null)
                {
                    Debug.LogError($"ListViewMenuController: ListView '{cfg.listViewName}' not found in the UI.");
                    _listViews.Add(null);
                    _displayNames.Add(new List<string>());
                    _keys.Add(new List<string>());
                    continue;
                }

                // Create and store separate name/key lists for this group
                var names = new List<string>();
                var keys = new List<string>();
                _displayNames.Add(names);
                _keys.Add(keys);
                _listViews.Add(lv);

                // Bind ListView to the names list
                lv.itemsSource = names;
                lv.makeItem = () => new Label();
                lv.bindItem = (ve, index) =>
                {
                    var label = (Label)ve;
                    label.text = (index >= 0 && index < names.Count) ? names[index] : "(none)";
                };
                lv.selectionType = SelectionType.Single;

                // Selection change → cascade to children
                int captured = i;
                lv.selectionChanged += _ => { CascadeChildren(captured); };
            }

            BuildDependencyMap();

            // Initial loads (roots first, then cascade)
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].dependsOnIndex < 0)
                {
                    ReloadGroup(i, parentKey: null);
                    CascadeChildren(i);
                }
            }
        }

        private void BuildDependencyMap()
        {
            _childrenOf = new Dictionary<int, List<int>>();
            for (int i = 0; i < groups.Count; i++)
            {
                int p = groups[i].dependsOnIndex;
                if (p >= 0 && p < groups.Count)
                {
                    if (!_childrenOf.TryGetValue(p, out var kids))
                    {
                        kids = new List<int>();
                        _childrenOf[p] = kids;
                    }
                    kids.Add(i);
                }
            }
        }

        private void CascadeChildren(int parentIndex)
        {
            if (!_childrenOf.TryGetValue(parentIndex, out var kids)) return;
            string parentKey = GetSelectedKey(parentIndex);

            foreach (int childIndex in kids)
            {
                ReloadGroup(childIndex, parentKey);
                CascadeChildren(childIndex);
            }
        }

        private void ReloadGroup(int groupIndex, string parentKey)
        {
            var cfg = groups[groupIndex];
            var lv = _listViews[groupIndex];
            if (lv == null) return;

            string path = string.IsNullOrEmpty(parentKey)
                ? cfg.baseResourcesPath
                : $"{cfg.baseResourcesPath}/{parentKey}";

            var assets = Resources.LoadAll<ScriptableObject>(path);

            var names = new List<string>();
            var keys = new List<string>();

            foreach (var so in assets)
            {
                if (so is IMenuSelection sel)
                {
                    string key = sel.GetMenuKey();
                    string display = sel.GetMenuText();
                    if (string.IsNullOrEmpty(key)) key = display;

                    if (!string.IsNullOrEmpty(display))
                    {
                        names.Add(display);
                        keys.Add(key);
                    }
                }
            }

            if (cfg.sortAlphabetically)
            {
                var zipped = names.Zip(keys, (n, k) => (n, k))
                                .OrderBy(t => t.n, StringComparer.OrdinalIgnoreCase)
                                .ToList();
                names = zipped.Select(t => t.n).ToList();
                keys = zipped.Select(t => t.k).ToList();
            }

            _displayNames[groupIndex].Clear();
            _displayNames[groupIndex].AddRange(names);

            _keys[groupIndex].Clear();
            _keys[groupIndex].AddRange(keys);

            lv.Rebuild();

            if (_displayNames[groupIndex].Count == 0)
            {
                lv.ClearSelection();
            }
            else if (cfg.selectFirstOnReload)
            {
                lv.SetSelection(0);
                lv.ScrollToItem(0);
            }
            else
            {
                int keep = Mathf.Clamp(lv.selectedIndex, 0, _displayNames[groupIndex].Count - 1);
                lv.SetSelection(keep);
                lv.ScrollToItem(keep);
            }
        }

        private string GetSelectedKey(int groupIndex)
        {
            var lv = _listViews[groupIndex];
            if (lv == null || lv.selectedIndex < 0) return null;

            var ks = _keys[groupIndex];
            return (lv.selectedIndex >= 0 && lv.selectedIndex < ks.Count) ? ks[lv.selectedIndex] : null;
        }
    }
}
