using UnityEngine;
using UnityEngine.SceneManagement;

namespace KotB.Menus
{
    public class SceneMenuItem : SimpleMenuItem
    {
        [Header("Scene Settings")]
        [SerializeField] private string sceneName;
        [SerializeField] private LoadSceneMode loadMode = LoadSceneMode.Single;

        public override void OnSelect()
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName, loadMode);
            }
            
            base.OnSelect();
        }
    }
}
