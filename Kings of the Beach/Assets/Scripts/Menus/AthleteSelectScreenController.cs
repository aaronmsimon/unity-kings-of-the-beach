using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace KotB.Menus.Alt
{
    public class AthleteSelectScreenController : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private InputReader inputReader;
        [SerializeField] private TeamSelectController[] teamSelectControllers;
        [SerializeField] private AthleteSelectController[] athleteSelectControllers;
        [SerializeField] private AthleteSelectionAtlasSO atlasSO;

        public int activeAthleteSelectControllerIndex;

        private List<VisualElement> elements;
        private int teamIndex;
        private int[] teamAthleteIndex = new int[] { 0, 0 };
        private bool statsVisible;

        private void Awake() {
            inputReader.EnableMenuInput();

            elements = uiDocument.rootVisualElement.Query(className: "input-icon").ToList();
        }

        private void OnEnable() {
            inputReader.triggerLeftEvent += OnTriggerLeft;
            inputReader.triggerRightEvent += OnTriggerRight;
            inputReader.interaction1Event += OnSwitchAthlete;
            inputReader.interaction2Event += OnSwitchTeam;
            inputReader.interaction4Event += OnSetHumanControlled;
            inputReader.selectEvent += OnToggleStats;
            inputReader.startEvent += OnStart;

            inputReader.inputSchemeChangedEvent += OnInputSchemeChanged;

            foreach (TeamSelectController team in teamSelectControllers) {
                team.TeamChanged += OnTeamChanged;
            }

// #if UNITY_EDITOR
            OnInputSchemeChanged(inputReader.CurrentScheme);
// #endif
        }

        private void Start() {
            // Set defaults
            SetActiveAthleteSelectController(0);
            SetHumanControlled(0);

            teamIndex = 1;
            OnSwitchTeam();
            statsVisible = false;
        }

        private void OnDisable() {
            inputReader.triggerLeftEvent -= OnTriggerLeft;
            inputReader.triggerRightEvent -= OnTriggerRight;
            inputReader.interaction1Event -= OnSwitchAthlete;
            inputReader.interaction2Event -= OnSwitchTeam;
            inputReader.interaction4Event -= OnSetHumanControlled;
            inputReader.selectEvent -= OnToggleStats;
            inputReader.startEvent -= OnStart;

            inputReader.inputSchemeChangedEvent -= OnInputSchemeChanged;
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
            teamSelectControllers[teamIndex].Deactivate();
            teamIndex = 1 - teamIndex;
            teamSelectControllers[teamIndex].Activate();
            SetActiveAthleteSelectController(teamIndex * 2 + teamAthleteIndex[teamIndex]);
        }

        private void OnToggleStats() {
            statsVisible = !statsVisible;
            foreach(AthleteSelectController athleteSelectController in athleteSelectControllers) {
                athleteSelectController.DisplayStats(statsVisible);
            }
        }

        private void OnStart() {
            SceneManager.LoadScene("Testing - Game");
        }

        private void OnTeamChanged(int teamIndex, IMenuDisplayable country, SkillsSO blocker, SkillsSO defender) {
            athleteSelectControllers[teamIndex * 2].SetAthlete(country, blocker);
            athleteSelectControllers[teamIndex * 2 + 1].SetAthlete(country, defender);
        }

        private void SetHumanControlled(int index) {
            foreach(AthleteSelectController athleteSelectController in athleteSelectControllers) {
                athleteSelectController.SetComputerControlled(true);
                athleteSelectController.SetLabelText("Computer Controlled");
            }
            athleteSelectControllers[index].SetComputerControlled(false);
            athleteSelectControllers[index].SetLabelText("Human Controlled");
        }

        private void OnInputSchemeChanged(InputReader.InputScheme newScheme) {
            foreach (VisualElement el in elements) {
                foreach (InputReader.InputScheme scheme in System.Enum.GetValues(typeof(InputReader.InputScheme))) {
                    el.RemoveFromClassList(scheme.ToString().ToLower());
                }
                el.AddToClassList(newScheme.ToString().ToLower());

                var match = atlasSO.inputIcons.FirstOrDefault(icon => el.ClassListContains(icon.inputName));
                if (match == null) continue;

                var field = typeof(AthleteSelectionAtlasSO.InputSchemes).GetField(newScheme.ToString());
                var tex = field?.GetValue(match.inputSchemes) as Texture2D;

                if (tex != null)
                    el.style.backgroundImage = new StyleBackground(tex);
            }
        }
    }
}
