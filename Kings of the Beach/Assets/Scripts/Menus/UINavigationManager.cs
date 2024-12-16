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

    [Header("Input Mapping")]
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode selectKey = KeyCode.Return;

    [Header("Visual Feedback")]
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.yellow;

    private void Start()
    {
        ValidateNavigationSetup();
        UpdateVerticalHighlight();
    }

    private void Update()
    {
        HandleNavigation();
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

    private void HandleNavigation()
    {
        // Vertical Navigation
        if (Input.GetKeyDown(upKey))
        {
            NavigateVertical(-1);
        }
        else if (Input.GetKeyDown(downKey))
        {
            NavigateVertical(1);
        }

        // Horizontal Navigation
        var currentSection = navigableSections[currentVerticalIndex];
        if (currentSection.allowHorizontalNavigation)
        {
            if (Input.GetKeyDown(leftKey))
            {
                NavigateHorizontal(-1);
            }
            else if (Input.GetKeyDown(rightKey))
            {
                NavigateHorizontal(1);
            }
        }

        // Selection
        if (Input.GetKeyDown(selectKey))
        {
            SelectCurrentSection();
        }
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
