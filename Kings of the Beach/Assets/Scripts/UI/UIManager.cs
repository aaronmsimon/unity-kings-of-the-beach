using UnityEngine;
using KotB.Match;
using KotB.Menus;

public class UIManager : MonoBehaviour
{
    [Header("UI Prefabs")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject powerMeter;
    [SerializeField] private GameObject aimServe;
    [SerializeField] private PauseMenu pauseMenu;

    [Header("Scriptable Objects")]
    [SerializeField] private MatchInfoSO matchInfo;

    private void OnEnable() {
        matchInfo.TogglePause += OnTogglePause;
    }

    private void OnDisable() {
        matchInfo.TogglePause -= OnTogglePause;
    }

    public void OnShowPowerMeter() {
        // Instantiate the UI object
        GameObject uiInstance = Instantiate(powerMeter);

        // Set the UI object as a child of the Canvas
        uiInstance.transform.SetParent(canvas.transform, false); // 'false' keeps local scale/rotation intact

        // Set the position within the canvas
        RectTransform rectTransform = uiInstance.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector3.zero;
    }

    public void OnShowServeAim() {
        Instantiate(aimServe);
    }

    public void OnTogglePause(bool paused) {
        pauseMenu.Hide(!paused);
    }
}
