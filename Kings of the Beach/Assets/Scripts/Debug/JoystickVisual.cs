using System;
using UnityEngine;

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
            Vector2 inputSquare = CircleMappedToSquare(moveInput.x, moveInput.y);

            Vector2 newPos = new Vector2(inputRange.x / 2 * inputSquare.x, inputRange.y / 2 * inputSquare.y);
            joystickPos.anchoredPosition = newPos;
        }

        private Vector2 CircleMappedToSquare(float u, float v) {
            float u2 = u * u;
            float v2 = v * v;
            float tworoot2 = 2 * Mathf.Sqrt(2);
            float subtermX = 2 + u2 - v2;
            float subtermY = 2 - u2 + v2;
            float termX1 = subtermX + u * tworoot2;
            float termX2 = subtermX - u * tworoot2;
            float termY1 = subtermY + v * tworoot2;
            float termY2 = subtermY - v * tworoot2;

            float epsilon = 0.0000001f;
            if (termX1 < epsilon)
                termX1 = 0;
            if (termX2 < epsilon)
                termX2 = 0;
            if (termY1 < epsilon)
                termY1 = 0;
            if (termY2 < epsilon)
                termY2 = 0;

            float x = Mathf.Clamp(0.5f * Mathf.Sqrt(termX1) - 0.5f * Mathf.Sqrt(termX2), -1, 1);
            float y = Mathf.Clamp(0.5f * Mathf.Sqrt(termY1) - 0.5f * Mathf.Sqrt(termY2), -1, 1);

            return new Vector2(x, y);
        }

        //---- EVENT LISTENERS ----

        private void OnMove(Vector2 movement)
        {
            moveInput = movement;
        }
    }
}
