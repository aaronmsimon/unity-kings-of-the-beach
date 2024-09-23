using UnityEngine;
using KotB;

namespace KotB.Testing
{
    public class JoystickVisual : MonoBehaviour
    {
        [SerializeField] private RectTransform joystickRange;
        [SerializeField] private RectTransform joystickPos;

        [SerializeField] private InputReader inputReader;

        private Vector2 inputRange;
        private Vector2 moveInput;

        private void Awake() {
            inputRange = new Vector2(joystickRange.rect.width, joystickRange.rect.height);
        }

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
            UpdateJoystickVisual();
        }

        private void UpdateJoystickVisual() {
            Vector2 inputSquare = Helpers.CircleMappedToSquare(moveInput.x, moveInput.y);

            Vector2 newPos = new Vector2(inputRange.x / 2 * inputSquare.x, inputRange.y / 2 * inputSquare.y);
            joystickPos.anchoredPosition = newPos;
        }

        //---- EVENT LISTENERS ----

        private void OnMove(Vector2 movement)
        {
            moveInput = movement;
        }
    }
}
