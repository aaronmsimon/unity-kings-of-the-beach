using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace KotB.Menus.Alt
{
    public class TeamSelectController : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField][Range(1,2)] private int teamIndex;

        private MenuPanel panel;
        private VisualElement teamLogo;

        private const string TeamsPath = "Athletes/Olympics 2024/Male/Team Selections";

        private void Awake() {
            List<IMenuDisplayable> teams = new List<IMenuDisplayable>(Resources.LoadAll<TeamSO>(TeamsPath));
            BuildTeamPanel(teams);
            /* set default image */
        }

        private void Start() {
            OnPanelValueChanged(panel, panel.CurrentValue);
        }

        public void BuildTeamPanel(List<IMenuDisplayable> teams) {
            var team = uiDocument.rootVisualElement.Q($"team-name-{teamIndex}");
            teamLogo = team.Q(className: "team-logo");

            panel = new MenuPanel();
            panel.Populate(teams);
            panel.OnValueChanged += OnPanelValueChanged;
            panel.Activate();
            panel.AddToClassList("team-label");
            team.Add(panel);
        }

        private void OnPanelValueChanged(MenuPanel panel, IMenuDisplayable value) {
            TeamSO team = (TeamSO)value;
            teamLogo.style.backgroundImage = new StyleBackground(Background.FromSprite(team.Country.Flag));
        }

        public MenuPanel Panel => panel;
    }
}
