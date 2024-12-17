using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UINavigationManager : MonoBehaviour
{
    [Serializable]
    public class NavigationSection
    {
        public string sectionName;
        public GameObject sectionRoot;
        public bool isSelectable = true;
        public bool allowHorizontalNavigation = false;
        public List<GameObject> horizontalOptions;
        public int currentHorizontalIndex = 0;
        public Action<GameObject> onSelect;
        public Action<GameObject> onHorizontalChange;
    }

    [Header("Navigation Configuration")]
    public List<NavigationSection> navigableSections;
    public int currentVerticalIndex = 0;

    [Header("Input Reader")]
    [SerializeField] private InputReader inputReader;

    [Header("Visual Feedback")]
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.yellow;

    private void Start()
    {
        ValidateNavigationSetup();
        UpdateVerticalHighlight();
    }

    private void OnEnable() {
        inputReader.selectionUpEvent += OnSelectionUp;
        inputReader.selectionDownEvent += OnSelectionDown;
        inputReader.selectionLeftEvent += OnSelectionLeft;
        inputReader.selectionRightEvent += OnSelectionRight;
        inputReader.selectEvent += ConfirmSelection;
    }

    private void OnDisable() {
        inputReader.selectionUpEvent -= OnSelectionUp;
        inputReader.selectionDownEvent -= OnSelectionDown;
        inputReader.selectionLeftEvent -= OnSelectionLeft;
        inputReader.selectionRightEvent -= OnSelectionRight;
        inputReader.selectEvent -= ConfirmSelection;
    }

    private void ValidateNavigationSetup()
    {
        // Ensure all sections have at least one selectable item
        foreach (var section in navigableSections)
        {
            if (section.isSelectable && section.sectionRoot == null)
            {
                Debug.LogWarning($"Navigation section {section.sectionName} is marked as selectable but has no root object.");
            }

            if (section.allowHorizontalNavigation && section.horizontalOptions.Count == 0)
            {
                Debug.LogWarning($"Section {section.sectionName} allows horizontal navigation but has no horizontal options.");
            }
        }
    }

    private void OnSelectionUp() {
        NavigateVertical(-1);
    }

    private void OnSelectionDown() {
        NavigateVertical(1);
    }

    private void OnSelectionLeft() {
        HandleHorizontalNavigation(-1);
    }

    private void OnSelectionRight() {
        HandleHorizontalNavigation(1);
    }

    private void HandleHorizontalNavigation(int direction)
    {
        var currentSection = navigableSections[currentVerticalIndex];
        if (currentSection.allowHorizontalNavigation)
        {
            NavigateHorizontal(direction);
        }
    }

    private void ConfirmSelection() {
        SelectCurrentSection();
    }

    private void NavigateVertical(int direction)
    {
        // Find next selectable section
        int attempts = navigableSections.Count;
        do
        {
            // Wrap around navigation
            currentVerticalIndex = (currentVerticalIndex + direction + navigableSections.Count) % navigableSections.Count;
            attempts--;
        } 
        while (!navigableSections[currentVerticalIndex].isSelectable && attempts > 0);

        // Update highlights
        UpdateVerticalHighlight();
    }

    private void NavigateHorizontal(int direction)
    {
        var currentSection = navigableSections[currentVerticalIndex];
        
        if (currentSection.allowHorizontalNavigation && currentSection.horizontalOptions.Count > 0)
        {
            // Deselect current horizontal option
            SetItemHighlight(currentSection.horizontalOptions[currentSection.currentHorizontalIndex], false);

            // Update horizontal index
            currentSection.currentHorizontalIndex = 
                (currentSection.currentHorizontalIndex + direction + currentSection.horizontalOptions.Count) 
                % currentSection.horizontalOptions.Count;

            // Highlight new horizontal option
            SetItemHighlight(currentSection.horizontalOptions[currentSection.currentHorizontalIndex], true);

            // Trigger horizontal change callback if defined
            currentSection.onHorizontalChange?.Invoke(
                currentSection.horizontalOptions[currentSection.currentHorizontalIndex]
            );
        }
    }

    private void UpdateVerticalHighlight()
    {
        // Reset all section highlights
        for (int i = 0; i < navigableSections.Count; i++)
        {
            SetItemHighlight(navigableSections[i].sectionRoot, i == currentVerticalIndex);
        }
    }

    private void SetItemHighlight(GameObject item, bool isHighlighted)
    {
        if (item == null) return;

        // Flexible highlighting mechanism
        var image = item.GetComponent<Image>();
        var text = item.GetComponent<Text>();

        if (image != null)
            image.color = isHighlighted ? highlightColor : defaultColor;

        if (text != null)
            text.color = isHighlighted ? highlightColor : defaultColor;
    }

    private void SelectCurrentSection()
    {
        var currentSection = navigableSections[currentVerticalIndex];
        
        if (currentSection.isSelectable)
        {
            // If horizontal navigation is available, select the current horizontal option
            GameObject selectedItem = currentSection.allowHorizontalNavigation 
                ? currentSection.horizontalOptions[currentSection.currentHorizontalIndex] 
                : currentSection.sectionRoot;

            // Invoke selection callback if defined
            currentSection.onSelect?.Invoke(selectedItem);
        }
    }

    // Utility method to dynamically add navigation sections at runtime
    public void AddNavigationSection(NavigationSection section)
    {
        navigableSections.Add(section);
        ValidateNavigationSetup();
    }
}
