// SceneButtonHandler.cs - Handler for scene transition buttons
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KotB.Menus
{
    public class SceneButtonHandler : BaseMenuItemHandler
    {
        private string sceneName;
        private LoadSceneMode loadMode = LoadSceneMode.Single;

        protected override void OnInitialize()
        {
            // Get configuration from MenuItemComponent
            var menuItemComponent = FindMenuItemComponent();
            if (menuItemComponent != null)
            {
                var config = menuItemComponent.GetSceneConfig();
                sceneName = config.sceneName;
                loadMode = config.loadMode;
            }
        }

        private MenuItemComponent FindMenuItemComponent()
        {
            // Try to find MenuItemComponent in the scene that references this element
            var menuItemComponents = Object.FindObjectsOfType<MenuItemComponent>();
            foreach (var component in menuItemComponents)
            {
                if (component.GetTargetElement() == element)
                {
                    return component;
                }
            }
            return null;
        }

        public override void OnSelect()
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName, loadMode);
            }
        }

        public void SetScene(string scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            sceneName = scene;
            loadMode = mode;
        }
    }
}
