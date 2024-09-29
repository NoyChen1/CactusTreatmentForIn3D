using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CactusUI : MonoBehaviour
{
    [SerializeField] private Text dayNightText;
    [SerializeField] private Text waterText;
    [SerializeField] private Text oxygenText;
    [SerializeField] private Text dayText;
    private DayNightCycle dayNightCycle;
    [SerializeField] private Cactus cactus;

    private StringBuilder waterStringBuilder;
    private StringBuilder oxygenStringBuilder;

    private void Start()
    {
        dayNightCycle = DayNightCycle.Instance;

        waterStringBuilder = new StringBuilder("Water Level: ");
        oxygenStringBuilder = new StringBuilder("Oxygen Level: ");

        dayNightCycle.OnDayStarted += UpdateDayNightText;
        dayNightCycle.OnNightStarted += UpdateDayNightText;
        dayNightCycle.OnDayCounterChanged += UpdateDayCounterText;
    }


    private void OnDestroy()
    {
        dayNightCycle.OnDayStarted -= UpdateDayNightText;
        dayNightCycle.OnNightStarted -= UpdateDayNightText;
        dayNightCycle.OnDayCounterChanged -= UpdateDayCounterText;
    }


    void Update()
    {
        //dayNightText.text = !dayNightCycle.IsDay() ? "Night" : "Day";
        // dayText.text = "Day: " + dayNightCycle.daysCounter;

        updateWaterText();
        updateOxygenText();
        //waterText.text = "Water Level: " + Mathf.RoundToInt(cactus.water) + "%";
       // oxygenText.text = "Oxygen Level: " + Mathf.RoundToInt(cactus.oxygen) + "%";
    }

    private void updateOxygenText()
    {
        waterStringBuilder.Length = 0;  
        waterStringBuilder.Append("Water Level: ");
        waterStringBuilder.Append(Mathf.RoundToInt(cactus.water));
        waterStringBuilder.Append("%");
        waterText.text = waterStringBuilder.ToString();
    }

    private void updateWaterText()
    {
        oxygenStringBuilder.Length = 0;
        oxygenStringBuilder.Append("Oxygen Level: ");
        oxygenStringBuilder.Append(Mathf.RoundToInt(cactus.oxygen));
        oxygenStringBuilder.Append("%");
        oxygenText.text = oxygenStringBuilder.ToString();
    }

    private void UpdateDayNightText()
    {
        dayNightText.text = !dayNightCycle.IsDay() ? "Night" : "Day";
    }

    private void UpdateDayCounterText()
    {
        dayText.text = "Day: " + dayNightCycle.daysCounter;
    }


}
