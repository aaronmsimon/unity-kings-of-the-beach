using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using KotB.Items;

namespace KotB.Menus.Alt
{
    public class AthleteSelectController : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private UIDocument uiDocument;

        private List<PanelConfig> _panelConfigs = new();
        private List<MenuPanel> _panels = new();
        private int _activePanelIndex = 0;

        // Cached loaded lists for outfit panels (static, loaded once)
        private List<IMenuDisplayable> _outfitTops;
        private List<IMenuDisplayable> _outfitBottoms;

        // Current selections
        private CountrySO _selectedCountry;
        private SkillsSO _selectedAthlete;
        private MaterialSO _selectedOutfitTop;
        private MaterialSO _selectedOutfitBottom;

        private void Awake()
        {
            // Load static lists once
            _outfitTops = AthleteMenuLoader.LoadOutfitTops();
            _outfitBottoms = AthleteMenuLoader.LoadOutfitBottoms();

            // Build panel configs explicitly
            _panelConfigs = new List<PanelConfig>
            {
                new PanelConfig
                {
                    LoadValues = AthleteMenuLoader.LoadCountries,
                    OnSelectionChanged = OnClassChanged
                },
                new PanelConfig
                {
                    LoadValues = () => AthleteMenuLoader.LoadAthletes(_selectedCountry?.DisplayName),
                    OnSelectionChanged = OnSubclassChanged
                },
                new PanelConfig
                {
                    LoadValues = () => _outfitTops,
                    OnSelectionChanged = value => _selectedOutfitTop = value as MaterialSO
                },
                new PanelConfig
                {
                    LoadValues = () => _outfitBottoms,
                    OnSelectionChanged = value => _selectedOutfitBottom = value as MaterialSO
                }
            };
        }

        private void OnEnable()
        {
            inputReader.EnableMenuInput();

            inputReader.selectionUpEvent += OnSelectionUp;
            inputReader.selectionDownEvent += OnSelectionDown;
            inputReader.selectionLeftEvent += OnSelectionLeft;
            inputReader.selectionRightEvent += OnSelectionRight;
            inputReader.startEvent += OnStart;

            BuildPanels();
        }

        private void OnDisable()
        {
            inputReader.selectionUpEvent -= OnSelectionUp;
            inputReader.selectionDownEvent -= OnSelectionDown;
            inputReader.selectionLeftEvent -= OnSelectionLeft;
            inputReader.selectionRightEvent -= OnSelectionRight;
            inputReader.startEvent -= OnStart;
        }

        private void BuildPanels()
        {
            var container = uiDocument.rootVisualElement.Q(className: "athlete-select-container");
            _panels.Clear();

            foreach (var config in _panelConfigs)
            {
                var panel = new MenuPanel();
                panel.OnValueChanged += OnPanelValueChanged;
                container.Add(panel);
                _panels.Add(panel);
                config.Panel = panel;
            }

            // Populate first panel and cascade from there
            PopulatePanel(0);
            SetActivePanel(0);
        }

        private void PopulatePanel(int index)
        {
            var config = _panelConfigs[index];
            var values = config.LoadValues();
            _panels[index].Populate(values);

            // Immediately register the default selection
            if (values.Count > 0)
                config.OnSelectionChanged?.Invoke(values[0]);

            // Cascade to next panel if it exists
            if (index + 1 < _panelConfigs.Count)
                PopulatePanel(index + 1);
        }

        private void OnPanelValueChanged(MenuPanel panel, IMenuDisplayable value)
        {
            int index = _panels.IndexOf(panel);
            _panelConfigs[index].OnSelectionChanged?.Invoke(value);
        }

        private void OnClassChanged(IMenuDisplayable value)
        {
            _selectedCountry = value as CountrySO;

            // Cascade — repopulate subclass panel and everything downstream
            PopulatePanel(1);
        }

        private void OnSubclassChanged(IMenuDisplayable value)
        {
            _selectedAthlete = value as SkillsSO;

            // Apply default outfits from subclass
            if (_selectedAthlete != null)
            {
                int topIndex = AthleteMenuLoader.FindDefaultIndex(_outfitTops, _selectedAthlete.DefaultTop);
                int bottomIndex = AthleteMenuLoader.FindDefaultIndex(_outfitBottoms, _selectedAthlete.DefaultBottom);

                _panels[2].ResetToDefault(topIndex);
                _panels[3].ResetToDefault(bottomIndex);

                _selectedOutfitTop = _selectedAthlete.DefaultTop;
                _selectedOutfitBottom = _selectedAthlete.DefaultBottom;
            }
        }

        private void SetActivePanel(int index)
        {
            _panels[_activePanelIndex].Deactivate();
            _activePanelIndex = index;
            _panels[_activePanelIndex].Activate();
        }

        private void OnSelectionUp()
        {
            int next = (_activePanelIndex - 1 + _panels.Count) % _panels.Count;
            SetActivePanel(next);
        }

        private void OnSelectionDown()
        {
            int next = (_activePanelIndex + 1) % _panels.Count;
            SetActivePanel(next);
        }

        private void OnSelectionLeft()
        {
            var activePanel = _panels[_activePanelIndex];
            if (activePanel.HasValues)
                activePanel.HandleHorizontal(-1);
            else
                OnSelectionUp();
        }

        private void OnSelectionRight()
        {
            var activePanel = _panels[_activePanelIndex];
            if (activePanel.HasValues)
                activePanel.HandleHorizontal(1);
            else
                OnSelectionDown();
        }

        private void OnStart()
        {
            // Write selections to your SO here
            // e.g. characterSelectionSO.SetSelection(_selectedClass, _selectedSubclass, _selectedOutfitTop, _selectedOutfitBottom);
        }
    }
}
