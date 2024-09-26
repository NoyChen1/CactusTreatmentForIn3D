using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance { get; private set; }  // Singleton instance


    [SerializeField] private Light sunLight;
    public float dayDuration = 60f;  // Total time for a full day (in seconds)
    private float timeOfDay = 0f;
    [SerializeField] public LightState state = LightState.Day;
    public int daysCounter = 1;


    public Action OnDayStarted;
    public Action OnNightStarted;
    public Action OnDayCounterChanged;

    [SerializeField] private Cactus cactus;

    public enum LightState
    {
        Day,
        Night
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        OnDayStarted += cactus.SunshineEffect;
        OnNightStarted += cactus.SunsetEffect;

        OnDayStarted?.Invoke();
        OnDayCounterChanged?.Invoke();
    }

    private void OnDestroy()
    {
        OnDayStarted -= cactus.SunshineEffect;
        OnNightStarted -= cactus.SunsetEffect;
    }


    void Update()
    {
        timeOfDay += Time.deltaTime;

        if (timeOfDay < dayDuration / 2 && 
            state != LightState.Day)
        {
            state = LightState.Day;
            daysCounter++;
            
            OnDayCounterChanged?.Invoke();
            OnDayStarted?.Invoke();  // Trigger the day action
        }
        else if (timeOfDay >= dayDuration / 2 && state != LightState.Night)
        {
            state = LightState.Night;
            OnNightStarted?.Invoke();  // Trigger the night action
        }

        if (timeOfDay >= dayDuration)
        {
            timeOfDay = 0f; // Reset cycle
        }

        // Rotate sun to simulate day-night
        sunLight.transform.Rotate(Vector3.right, (360 / dayDuration) * Time.deltaTime);
    }
    internal bool IsDay()
    {
        return state == LightState.Day;
    }
}
