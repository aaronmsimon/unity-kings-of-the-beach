using UnityEngine;
using UnityEngine.UI;
using RoboRyanTron.Unite2017.Variables;

public class PowerMeter : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] private FloatVariable powerValue;
    
    [Header("Setup")]
    [SerializeField] private Image barImage;
    
    private float hideDelay = 0.5f;

    private void Update() {
        barImage.fillAmount = powerValue.Value;
    }

    public void OnHidePowerMeter() {
        Destroy(this.gameObject, hideDelay);
    }
}
