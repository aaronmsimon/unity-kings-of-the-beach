using UnityEngine;
using KotB.Actors;

public class Serving : MonoBehaviour
{
    [SerializeField] private Player player;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) {
            player.SetAsServer();
        }
    }
}
