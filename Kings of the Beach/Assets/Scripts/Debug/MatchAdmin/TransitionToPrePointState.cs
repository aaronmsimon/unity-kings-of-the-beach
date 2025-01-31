using UnityEngine;
using KotB.Match;

namespace KotB.Testing
{
    public class TransitionToPrePointState : MonoBehaviour
    {
        [SerializeField] private MatchInfoSO matchInfo;
        [SerializeField] private InputReader inputReader;

        private void Awake() {
            inputReader.EnableBetweenPointsInput();
        }

        private void OnEnable() {
            inputReader.interactEvent += OnInteract;
        }

        private void OnDisable() {
            inputReader.interactEvent -= OnInteract;
        }

        private void OnInteract() {
            matchInfo.TransitionToPrePointStateEvent();
            inputReader.EnableGameplayInput();
        }
    }
}
