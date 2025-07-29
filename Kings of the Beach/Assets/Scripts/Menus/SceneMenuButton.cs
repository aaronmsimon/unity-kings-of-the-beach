// SceneMenuButton.cs - Menu button for scene transitions
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace KotB.Menus
{
    public class SceneMenuButton : SimpleMenuButton
    {
        private string sceneName;
        private LoadSceneMode loadMode = LoadSceneMode.Single;

        public SceneMenuButton() : base() { }

        public SceneMenuButton(string text, string targetScene, LoadSceneMode mode = LoadSceneMode.Single) 
            : base(text, null)
        {
            sceneName = targetScene;
            loadMode = mode;
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

        public string SceneName => sceneName;
    }
}
