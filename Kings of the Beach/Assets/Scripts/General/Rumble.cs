using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cackenballz.Helpers
{
    public static class Rumble
    {
        public static IEnumerator RumbleCoroutine(float duration, float leftMotor, float rightMotor) {
            if (Gamepad.current != null) {
                Gamepad.current?.SetMotorSpeeds(leftMotor, rightMotor);
                yield return new WaitForSeconds(duration);
                Gamepad.current?.ResetHaptics();
            }
        }
    }
}
