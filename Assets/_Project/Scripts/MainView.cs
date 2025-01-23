using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    [SerializeField] private Slider powerSlider;
    [SerializeField] private TMP_Text powerText;
    [SerializeField] private CannonConfigSO cannonConfig;

    private void Awake()
    {
        powerSlider.value = cannonConfig.power;
        powerText.text = cannonConfig.power.Value.ToString();
    }

    private void OnEnable()
    {
        cannonConfig.power.AddListener(OnPowerChange);
    }

    private void OnDisable()
    {
        cannonConfig.power.RemoveListener(OnPowerChange);
    }
    
    private void OnPowerChange(float value)
    {
        int intValue = Mathf.RoundToInt(value);
        powerSlider.value = value;
        powerText.text = intValue.ToString();
    }
}
