using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        dayNightCycle = DayNightCycle.Instance;

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
        waterText.text = "Water Level: " + Mathf.RoundToInt(cactus.water) + "%";
        oxygenText.text = "Oxygen Level: " + Mathf.RoundToInt(cactus.oxygen) + "%";
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
