using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace KotB.Testing
{
    public class JoystickVisual : MonoBehaviour
    {
        [SerializeField] private RectTransform joystickRange;
        [SerializeField] private RectTransform joystickPos;

        private Vector2 inputRange;
        private Vector2 moveInput;

        private PlayerControls playerControls;

        private void Awake() {
            inputRange = new Vector2(joystickRange.rect.width, joystickRange.rect.height);

            playerControls = new PlayerControls();
        }

        private void OnEnable() {
            playerControls.Enable();
        }

        private void OnDisable() {
            playerControls.Disable();
        }

        private void Update() {
            GetMoveInput();

            // Update Joystick Visual
            Vector2 newPos = new Vector2(inputRange.x / 2 * moveInput.x, inputRange.y / 2 * moveInput.y);
            joystickPos.anchoredPosition = newPos;
        }

        private void GetMoveInput() {
            moveInput = playerControls.Gameplay.Move.ReadValue<Vector2>();
        }
    }
}
