using UnityEngine;
using UnityEngine.UIElements;

namespace KotB.Menus.Alt
{
    public class AthleteSelectScreenController : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private InputReader inputReader;
        [SerializeField] private AthleteSelectController[] athleteSelectControllers;

        public int activeAthleteSelectControllerIndex;

        private void Awake() {
            inputReader.EnableMenuInput();
        }

        private void OnEnable() {
            inputReader.shoulderLeftEvent += OnShoulderLeft;
            inputReader.shoulderRightEvent += OnShoulderRight;
            inputReader.interaction1Event += OnInteraction1;
        }

        private void Start() {
            foreach(AthleteSelectController athleteSelectController in athleteSelectControllers) {
                athleteSelectController.BuildPanels(uiDocument);
            }
            SetActiveAthleteSelectController(0);
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

        private void OnInteraction1() {
            foreach(AthleteSelectController athleteSelectController in athleteSelectControllers) {
                athleteSelectController.AthleteConfig.computerControlled = true;
            }
            athleteSelectControllers[activeAthleteSelectControllerIndex].AthleteConfig.computerControlled = false;
        }
    }
}
