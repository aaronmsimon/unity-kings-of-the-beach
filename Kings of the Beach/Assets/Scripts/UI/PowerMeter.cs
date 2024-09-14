using UnityEngine;
using UnityEngine.UI;

public class PowerMeter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float fillSpeed;
    [SerializeField] private float drainSpeed;

    [Header("Setup")]
    [SerializeField] private Image barImage;

    private float powerValue;
    private bool isActive;
    private bool isIncreasing;

    public float PowerValue {
        get {
            return powerValue;
        }
    }

    private void Start() {
        powerValue = 0;
        StopMeter();
    }

    private void Update() {
        if (isActive) {
            powerValue += (isIncreasing ? fillSpeed : -drainSpeed) * Time.deltaTime;
            powerValue = Mathf.Clamp01(powerValue);
            if (powerValue == 1) isIncreasing = false;
            if (powerValue == 0) StopMeter();

        }

        barImage.fillAmount = powerValue;

        /* temp */
        if (Input.GetKeyDown(KeyCode.V)) {
            Interact();
        }
    }

    public void Interact() {
        if (!isActive) {
            StartMeter();
        } else {
            StopMeter();
        }
    }

    private void StartMeter() {
        powerValue = 0;
        isIncreasing = true;
        isActive = true;
    }

    private void StopMeter() {
        isActive = false;
        Debug.Log("Power is " + powerValue);
    }
}
