using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor;

namespace KotB.Menus.Alt
{
    public class MainMenuController : MenuController
    {
        [SerializeField] private List<MainMenuItemData> menuItems;

        private List<MenuPanel> _panels = new();
        private int _activePanelIndex = 0;

        protected override void OnEnable() {
            base.OnEnable();
            BuildMenu();
        }

        private void BuildMenu()
        {
            var root = uiDocument.rootVisualElement.Q("root");
            _panels.Clear();

            foreach (var item in menuItems)
            {
                var panel = new MenuPanel();
                panel.Populate(new List<IMenuDisplayable> { item });
                root.Add(panel);
                _panels.Add(panel);
            }

            SetActivePanel(0);
        }

        private void SetActivePanel(int index)
        {
            _panels[_activePanelIndex].Deactivate();
            _activePanelIndex = index;
            _panels[_activePanelIndex].Activate();
        }

        protected override void OnSelectionUp() {
            int next = (_activePanelIndex - 1 + _panels.Count) % _panels.Count;
            SetActivePanel(next);
        }

        protected override void OnSelectionDown() {
            int next = (_activePanelIndex + 1) % _panels.Count;
            SetActivePanel(next);
        }

        // private void HandleLeft() => _menuPanel.HandleVertical(-1);
        // private void HandleRight() => _menuPanel.HandleVertical(1);

        protected override void OnStart() {
            MainMenuItemData selected = (MainMenuItemData)_panels[_activePanelIndex].CurrentValue;
            if (selected == null) return;

            if (string.IsNullOrEmpty(selected.SceneName))
            {
                Application.Quit();
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
                return;
            }

            SceneManager.LoadScene(selected.SceneName);
        }

        [Serializable]
        private class MainMenuItemData : IMenuDisplayable
        {
            [SerializeField] private string displayName;
            [SerializeField] private string sceneName;

            public string DisplayName => displayName;
            public string SceneName => sceneName;
        }
    }
}
