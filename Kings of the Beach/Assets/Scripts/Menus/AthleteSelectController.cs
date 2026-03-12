using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using KotB.Items;
using KotB.Match;

namespace KotB.Menus.Alt
{
    public class AthleteSelectController : MenuController
    {
        [SerializeField] private int uiDocumentIndex;
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] private AthleteConfigSO athleteConfig;
        [SerializeField] private Camera cam;

        public event Action<MaterialSO> OutfitTopChanged;
        public event Action<MaterialSO> OutfitBottomChanged;

        private List<PanelConfig> panelConfigs = new();

        // Cached loaded lists for outfit panels (static, loaded once)
        private List<IMenuDisplayable> outfitTops;
        private List<IMenuDisplayable> outfitBottoms;

        // Current selections
        private CountrySO selectedCountry;
        private SkillsSO selectedAthlete;

        // Panel indices
        private const int COUNTRY_PANEL = 0;
        private const int ATHLETE_PANEL = 1;
        private const int OUTFIT_TOP_PANEL = 2;
        private const int OUTFIT_BOT_PANEL = 3;

        private void Awake() {
            // Check if prefab has been configured
            if (renderTexture == null) Debug.LogAssertion($"No render texture for {transform.parent.name}.");
            if (athleteConfig == null) Debug.LogAssertion($"No athlete configuration for {transform.parent.name}.");
            if (cam.targetTexture == null) Debug.LogAssertion($"No render texture (camera) for {transform.parent.name}.");

            // Load static lists once
            outfitTops = AthleteMenuLoader.LoadOutfitTops();
            outfitBottoms = AthleteMenuLoader.LoadOutfitBottoms();

            // Build panel configs explicitly
            panelConfigs = new List<PanelConfig> {
                new PanelConfig {
                    LoadValues = AthleteMenuLoader.LoadCountries,
                    OnSelectionChanged = OnCountryChanged
                },
                new PanelConfig {
                    LoadValues = () => AthleteMenuLoader.LoadAthletes(selectedCountry?.DisplayName),
                    OnSelectionChanged = OnAthleteChanged
                },
                new PanelConfig {
                    LoadValues = () => outfitTops,
                    OnSelectionChanged = OnTopChanged
                },
                new PanelConfig {
                    LoadValues = () => outfitBottoms,
                    OnSelectionChanged = OnBottomChanged
                }
            };
        }

        private void Start() {
            // Load defaults
            OnAthleteChanged(selectedAthlete);
        }

        public void BuildPanels(UIDocument uiDocument) {
            var slot = uiDocument.rootVisualElement.Q($"athlete-slot-{uiDocumentIndex}");
            var selectionsContainer = slot.Q(className: "athlete-select-container");
            var textureContainer = slot.Q(className: "render-texture-container");

            panels.Clear();

            foreach (var config in panelConfigs) {
                var panel = new MenuPanel();
                panel.OnValueChanged += OnPanelValueChanged;
                selectionsContainer.Add(panel);
                panels.Add(panel);
                config.Panel = panel;
            }

            // Display render texture
            textureContainer.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(renderTexture));

            // Populate panels
            PopulatePanel(COUNTRY_PANEL);
            PopulatePanel(OUTFIT_TOP_PANEL);
            PopulatePanel(OUTFIT_BOT_PANEL);
            SetActivePanel(COUNTRY_PANEL);
        }

        private void PopulatePanel(int index) {
            var config = panelConfigs[index];
            var values = config.LoadValues();
            panels[index].Populate(values);

            // Immediately register the default selection
            if (values.Count > 0) config.OnSelectionChanged?.Invoke(values[0]);
        }

        private void OnPanelValueChanged(MenuPanel panel, IMenuDisplayable value) {
            int index = panels.IndexOf(panel);
            panelConfigs[index].OnSelectionChanged?.Invoke(value);
        }

        private void OnCountryChanged(IMenuDisplayable value) {
            selectedCountry = (CountrySO)value;

            // Cascade — repopulate Athlete panel and everything downstream
            PopulatePanel(ATHLETE_PANEL);
        }

        private void OnAthleteChanged(IMenuDisplayable value)
        {
            selectedAthlete = (SkillsSO)value;

            // Apply default outfits from Athlete
            if (selectedAthlete != null) {
                int defaultTop = AthleteMenuLoader.FindDefaultIndex(outfitTops, selectedAthlete.DefaultTop);
                int defaultBottom = AthleteMenuLoader.FindDefaultIndex(outfitBottoms, selectedAthlete.DefaultBottom);

                panels[OUTFIT_TOP_PANEL].ResetToDefault(defaultTop);
                OnTopChanged(outfitTops[defaultTop]);
                panels[OUTFIT_BOT_PANEL].ResetToDefault(defaultBottom);
                OnBottomChanged(outfitBottoms[defaultBottom]);

                athleteConfig.skills = selectedAthlete;
            }
        }

        private void OnTopChanged(IMenuDisplayable value) {
            MaterialSO mat = (MaterialSO)value;
            OutfitTopChanged?.Invoke(mat);
            athleteConfig.top = mat;
        }

        private void OnBottomChanged(IMenuDisplayable value) {
            MaterialSO mat = (MaterialSO)value;
            OutfitBottomChanged?.Invoke((MaterialSO)value);
            athleteConfig.bottom = mat;
        }

        protected override void OnStart()
        {
            // Write selections to your SO here
            // e.g. characterSelectionSO.SetSelection(_selectedClass, _selectedSubclass, _selectedOutfitTop, _selectedOutfitBottom);
        }

        private class PanelConfig
        {
            public MenuPanel Panel;
            public Func<List<IMenuDisplayable>> LoadValues;
            public Action<IMenuDisplayable> OnSelectionChanged; // null if static
        }
    }
}
