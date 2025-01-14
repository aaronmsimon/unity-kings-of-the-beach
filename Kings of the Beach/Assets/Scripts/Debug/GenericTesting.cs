using UnityEngine;
using KotB.Match;
using KotB.Actors;

namespace KotB.Testing
{
    public class GenericTesting : MonoBehaviour
    {
        [SerializeField] private MatchInfoSO matchInfo;
        
        private void Update() {
            if (Input.GetKeyDown(KeyCode.J)) {
                Debug.Log(matchInfo.GetServer());
            }
        }
    }
}
