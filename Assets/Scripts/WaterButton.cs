using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterButton : MonoBehaviour
{
    [SerializeField] private Cactus cactus;
    [SerializeField] private Button waterButton;

    private void Start()
    {
        waterButton.onClick.AddListener(OnWaterButtonClick);
    }

    public void OnWaterButtonClick()
    {
        cactus.WaterCactus();
        waterButton.interactable = false;
        StartCoroutine(ReenableButtonAfterDelay(2f));
       // cactus.afterWater();
    }

    private IEnumerator ReenableButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        waterButton.interactable = true;
    }
}
