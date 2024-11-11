using UnityEngine;
using UnityEngine.SceneManagement;

namespace KotB.Menus
{
    public class TitleScreen : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;

        private void Awake() {
            inputReader.EnableMenuInput();
        }

        private void OnEnable() {
            inputReader.selectEvent += OnStart;
            inputReader.startEvent += OnStart;
        }

        private void OnDisable() {
            inputReader.selectEvent -= OnStart;
            inputReader.startEvent -= OnStart;
        }

        private void OnStart() {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}
