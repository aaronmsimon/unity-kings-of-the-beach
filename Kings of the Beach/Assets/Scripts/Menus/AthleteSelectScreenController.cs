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
        private int[] teamAthleteIndex = new int[] { 0, 0 };

        private void Awake() {
            inputReader.EnableMenuInput();
        }

        private void OnEnable() {
            inputReader.triggerLeftEvent += OnTriggerLeft;
            inputReader.triggerRightEvent += OnTriggerRight;
            inputReader.selectEvent += OnSetHumanControlled;
            inputReader.interaction1Event += OnSwitchAthlete;
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
            inputReader.triggerLeftEvent -= OnTriggerLeft;
            inputReader.triggerRightEvent -= OnTriggerRight;
            inputReader.selectEvent -= OnSetHumanControlled;
            inputReader.interaction1Event -= OnSwitchAthlete;
            inputReader.interaction2Event -= OnSwitchTeam;
        }

        private void SetActiveAthleteSelectController(int index) {
            athleteSelectControllers[activeAthleteSelectControllerIndex].Deactivate();
            activeAthleteSelectControllerIndex = index;
            athleteSelectControllers[activeAthleteSelectControllerIndex].Activate();
        }

        private void OnSwitchAthlete() {
            teamAthleteIndex[teamIndex] = 1 - teamAthleteIndex[teamIndex];
            SetActiveAthleteSelectController(teamIndex * 2 + teamAthleteIndex[teamIndex]);
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
            SetActiveAthleteSelectController(teamIndex * 2 + teamAthleteIndex[teamIndex]);
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
