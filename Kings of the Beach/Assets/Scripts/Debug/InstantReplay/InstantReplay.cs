using UnityEngine;

namespace KotB.Testing
{
    public class InstantReplay : MonoBehaviour
    {
        [SerializeField] private Cinemachine.InstantReplay instantReplay;

        public void RunInstantReplay() {
            instantReplay.Play();
        }
    }
}
