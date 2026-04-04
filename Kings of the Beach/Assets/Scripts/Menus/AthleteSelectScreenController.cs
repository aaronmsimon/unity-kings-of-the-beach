using UnityEngine;
using UnityEngine.UIElements;

namespace KotB.Menus.Alt
{
    public class AthleteSelectScreenController : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private InputReader inputReader;
        [SerializeField] private TeamSelectController[] teamSelectControllers;
        [SerializeField] private AthleteSelectController[] athleteSelectControllers;

        public int activeAthleteSelectControllerIndex;

        private int teamIndex;

        private void Awake() {
            inputReader.EnableMenuInput();
        }

        private void OnEnable() {
            inputReader.shoulderLeftEvent += OnShoulderLeft;
            inputReader.shoulderRightEvent += OnShoulderRight;
            inputReader.triggerLeftEvent += OnTriggerLeft;
            inputReader.triggerRightEvent += OnTriggerRight;
            inputReader.interaction1Event += OnSetHumanControlled;
            inputReader.interaction2Event += OnSwitchTeam;
        }

        private void Start() {
            foreach(AthleteSelectController athleteSelectController in athleteSelectControllers) {
                athleteSelectController.BuildPanels(uiDocument);
            }

            // Set defaults
            SetActiveAthleteSelectController(0);
            SetHumanControlled(0);

            teamIndex = 0;
        }

        private void OnDisable() {
            inputReader.shoulderLeftEvent -= OnShoulderLeft;
            inputReader.shoulderRightEvent -= OnShoulderRight;
            inputReader.triggerLeftEvent -= OnTriggerLeft;
            inputReader.triggerRightEvent -= OnTriggerRight;
            inputReader.interaction1Event -= OnSetHumanControlled;
            inputReader.interaction2Event -= OnSwitchTeam;
        }

        private void SetActiveAthleteSelectController(int index) {
            athleteSelectControllers[activeAthleteSelectControllerIndex].Deactivate();
            activeAthleteSelectControllerIndex = index;
            athleteSelectControllers[activeAthleteSelectControllerIndex].Activate();
        }

        private void OnShoulderLeft() {
            SwitchAthlete(-1);
        }

        private void OnShoulderRight() {
            SwitchAthlete(1);
        }

        private void SwitchAthlete(int direction) {
            SetActiveAthleteSelectController((activeAthleteSelectControllerIndex + direction + athleteSelectControllers.Length) % athleteSelectControllers.Length);
        }

        private void OnTriggerLeft() {
            SelectTeam(-1);
        }

        private void OnTriggerRight() {
            SelectTeam(1);
        }

        private void SelectTeam(int direction) {
            teamSelectControllers[teamIndex].Panel.HandleHorizontal(direction);
        }

        private void OnSetHumanControlled() {
            SetHumanControlled(activeAthleteSelectControllerIndex);
        }

        private void OnSwitchTeam() {
            teamIndex = 1 - teamIndex;
        }

        private void SetHumanControlled(int index) {
            foreach(AthleteSelectController athleteSelectController in athleteSelectControllers) {
                athleteSelectController.SetComputerControlled(true);
                athleteSelectController.SetLabelText("Computer Controlled");
            }
            athleteSelectControllers[index].SetComputerControlled(false);
            athleteSelectControllers[index].SetLabelText("Human Controlled");
        }
    }
}
