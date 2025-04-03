using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class PlayerBlockTesting : MonoBehaviour
    {
        [SerializeField] private Player player;

        private void Start() {
            ResetStates();
        }

        private void Update() {
            if (Input.GetKey(KeyCode.Space)) {
                ResetStates();
            }
        }

        private void ResetStates() {
            player.GetComponent<SetAthleteState>().SetState();
        }
    }
}
