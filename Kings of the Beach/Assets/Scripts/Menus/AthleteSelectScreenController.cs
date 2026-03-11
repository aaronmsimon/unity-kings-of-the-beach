using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace KotB.Menus.Alt
{
    public class AthleteSelectScreenController : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private AthleteSelectController[] athleteSelectControllers;

        public int activeAthleteSelectControllerIndex;

        private void Awake() {
            inputReader.EnableMenuInput();
        }

        private void OnEnable() {
            inputReader.shoulderLeftEvent += OnShoulderLeft;
            inputReader.shoulderRightEvent += OnShoulderRight;
        }

        private void OnDisable() {
            inputReader.shoulderLeftEvent -= OnShoulderLeft;
            inputReader.shoulderRightEvent -= OnShoulderRight;
        }

        private void SetActiveAthleteSelectController(int index) {
            athleteSelectControllers[activeAthleteSelectControllerIndex].Deactivate();
            activeAthleteSelectControllerIndex = index;
            athleteSelectControllers[activeAthleteSelectControllerIndex].Activate();
        }

        private void OnShoulderLeft() {
            SelectionChange(-1);
        }

        private void OnShoulderRight() {
            SelectionChange(1);
        }

        private void SelectionChange(int direction) {
            SetActiveAthleteSelectController((activeAthleteSelectControllerIndex + direction + athleteSelectControllers.Length) % athleteSelectControllers.Length);
        }
    }
}
