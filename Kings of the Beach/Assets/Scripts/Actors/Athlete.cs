using UnityEngine;

namespace KotB.Actors
{
    public class Athlete : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        [SerializeField] private InputReader inputReader;

        private Vector2 moveInput;

        //Adds listeners for events being triggered in the InputReader script
        private void OnEnable()
        {
            inputReader.moveEvent += OnMove;
        }
        
        //Removes all listeners to the events coming from the InputReader script
        private void OnDisable()
        {
            inputReader.moveEvent -= OnMove;
        }

        private void Update() {
            Move();
        }

        private void Move() {
            Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        //---- EVENT LISTENERS ----

        private void OnMove(Vector2 movement)
        {
            moveInput = movement;
        }
    }
}