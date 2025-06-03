using UnityEngine;
using KotB.Actors;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Testing
{
    public class LoadTransitionProfile : MonoBehaviour
    {
        [SerializeField] private StringVariable playerTransitionProfile;

        private void Start() {
            Player player = FindObjectOfType<Player>();
            player.StateMachine.SetActiveProfile(playerTransitionProfile.Value);
        }
    }
}
