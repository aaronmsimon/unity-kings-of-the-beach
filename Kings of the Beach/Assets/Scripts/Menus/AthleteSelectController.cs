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

        // Panel configuration — explicit cascade relationships
        private class PanelConfig
        {
            public MenuPanel Panel;
            public Func<List<IMenuDisplayable>> LoadValues;
            public Action<IMenuDisplayable> OnSelectionChanged; // null if static
        }

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

            inputReader.selectionUpEvent += HandleUp;
            inputReader.selectionDownEvent += HandleDown;
            inputReader.selectionLeftEvent += HandleLeft;
            inputReader.selectionRightEvent += HandleRight;
            inputReader.startEvent += HandleStart;

            BuildPanels();
        }

        private void OnDisable()
        {
            inputReader.selectionUpEvent -= HandleUp;
            inputReader.selectionDownEvent -= HandleDown;
            inputReader.selectionLeftEvent -= HandleLeft;
            inputReader.selectionRightEvent -= HandleRight;
            inputReader.startEvent -= HandleStart;
        }

        private void BuildPanels()
        {
            var root = uiDocument.rootVisualElement;
            _panels.Clear();

            foreach (var config in _panelConfigs)
            {
                var panel = new MenuPanel();
                panel.OnValueChanged += OnPanelValueChanged;
                root.Add(panel);
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

        private void HandleUp()
        {
            int next = (_activePanelIndex - 1 + _panels.Count) % _panels.Count;
            SetActivePanel(next);
        }

        private void HandleDown()
        {
            int next = (_activePanelIndex + 1) % _panels.Count;
            SetActivePanel(next);
        }

        private void HandleLeft()
        {
            var activePanel = _panels[_activePanelIndex];
            if (activePanel.HasValues)
                activePanel.HandleHorizontal(-1);
            else
                HandleUp();
        }

        private void HandleRight()
        {
            var activePanel = _panels[_activePanelIndex];
            if (activePanel.HasValues)
                activePanel.HandleHorizontal(1);
            else
                HandleDown();
        }

        private void HandleStart()
        {
            // Write selections to your SO here
            // e.g. characterSelectionSO.SetSelection(_selectedClass, _selectedSubclass, _selectedOutfitTop, _selectedOutfitBottom);
        }
    }
}
