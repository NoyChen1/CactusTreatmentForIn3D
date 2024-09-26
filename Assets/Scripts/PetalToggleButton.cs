using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetalToggleButton : MonoBehaviour
{
    [SerializeField] private Cactus cactus;
    [SerializeField] private Button toggleButton;
    [SerializeField] private Text toggleText;

    private DayNightCycle dayNightCycle;

    private void Start()
    {
        dayNightCycle = DayNightCycle.Instance;
        toggleButton.onClick.AddListener(OnTogglePetalsButtonClick);

       // dayNightCycle.OnDayStarted += UpdateButtonVisibility;
      //  dayNightCycle.OnNightStarted += UpdateButtonVisibility;

    }
    
    private void Update()
    {
        if ((cactus.isPetalsOpen() && dayNightCycle.IsDay()) || //petals open and day
           (!cactus.isPetalsOpen() && !dayNightCycle.IsDay())) //petal close and night
        {
            toggleButton.gameObject.SetActive(false);
        }

        if (cactus.isPetalsOpen() && !dayNightCycle.IsDay()) // petals open and night
        {
            toggleButton.gameObject.SetActive(true);
            toggleText.text = "Close Petals";
        }
        else if (!cactus.isPetalsOpen() && dayNightCycle.IsDay()) //petals close and day
        {
            toggleButton.gameObject.SetActive(true);
            toggleText.text = "Open Petals";
        }
        else
        {
            toggleButton.gameObject.SetActive(false);
        }
    }


    /*private void UpdateButtonVisibility()
    {
        if ((cactus.isPetalsOpen() && dayNightCycle.IsDay()) || //petals open and day
           (!cactus.isPetalsOpen() && !dayNightCycle.IsDay())) //petal close and night
        {
            toggleButton.gameObject.SetActive(false);
        }

        if (cactus.isPetalsOpen() && !dayNightCycle.IsDay()) // petals open and night
        {
            toggleButton.gameObject.SetActive(true);
            toggleText.text = "Close Petals";
        }
        else if (!cactus.isPetalsOpen() && dayNightCycle.IsDay()) //petals close and day
        {
            toggleButton.gameObject.SetActive(true);
            toggleText.text = "Open Petals";
        }
        else
        {
            toggleButton.gameObject.SetActive(false);
        }
    }*/

    public void OnTogglePetalsButtonClick()
    {
        cactus.TogglePetals();
    }
}
