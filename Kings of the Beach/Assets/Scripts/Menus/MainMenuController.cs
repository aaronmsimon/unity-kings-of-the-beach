using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace KotB.Menus.Alt
{
    public class MainMenuController : MenuController
    {
        [SerializeField] private List<MainMenuItemData> menuItems;

        private MenuPanel _menuPanel;

        protected override void OnEnable() {
            BuildMenu();
        }

        private void BuildMenu()
        {
            var root = uiDocument.rootVisualElement.Q("root");

            foreach (MainMenuItemData menuItem in menuItems) {
                VisualElement el = new VisualElement();
                el.Add(new Label(menuItem.DisplayName));
                root.Add(el);
            }
        }

        // private void HandleUp() => _menuPanel.HandleVertical(-1);
        // private void HandleDown() => _menuPanel.HandleVertical(1);
        // private void HandleLeft() => _menuPanel.HandleVertical(-1);
        // private void HandleRight() => _menuPanel.HandleVertical(1);

        protected override void OnStart() {
            var selected = _menuPanel.CurrentValue as MainMenuItemData;
            Debug.Log(selected);
            if (selected == null) return;

            if (selected.SceneName == null)
            {
                Application.Quit();
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
