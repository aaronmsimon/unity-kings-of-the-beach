using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class UIManager : MonoBehaviour
{
    [Header("UI Prefabs")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject powerMeter;
    [SerializeField] private GameObject aimServe;
    [SerializeField] private GameObject pauseMenu;

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

    public void OnPause() {
        pauseMenu.SetActive(true);
    }

    public void OnUnpause() {
        pauseMenu.SetActive(false);
    }
}
