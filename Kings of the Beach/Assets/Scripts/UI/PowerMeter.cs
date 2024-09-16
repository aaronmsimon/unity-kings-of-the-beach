using UnityEngine;
using UnityEngine.UI;
using RoboRyanTron.Unite2017.Variables;

public class PowerMeter : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] private FloatVariable powerValue;
    
    [Header("Setup")]
    [SerializeField] private Image barImage;

    private void Update() {
        barImage.fillAmount = powerValue.Value;
    }
}
