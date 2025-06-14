using UnityEngine;
using KotB.Actors;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Testing
{
    public class LoadTransitionProfile : MonoBehaviour
    {
        [SerializeField] private StringVariable transitionProfile;

        private void Start() {
            GetComponent<Athlete>().StateMachine.SetActiveProfile(transitionProfile.Value);
        }
    }
}
