using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class Cactus : MonoBehaviour
{
    public float water = 100f;
    public float oxygen = 100f;

    public Renderer cactusRenderer;
    [SerializeField] private ParticleSystem waterEffect;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Animator animator;

    private DayNightCycle dayNightCycle;

    private int spikesIndex = 0;
    private int flowerIndex = 1;

    private float oxygenTimer = 0f;
    private float oxygenLvl = 0f;
    private float waterTimer = 0f;
    private float interval = 1f;
    [SerializeField] private bool petalsOpen;

    private Vector2 happyOffset = new Vector2(0.5f, 0); 
    private Vector2 sadOffset = new Vector2(0, 0);
    private State cactusState = State.Happy;

    [SerializeField] private Text cactusDieText;

    public Action<float> OnWaterChanged;
    public Action<float> OnOxygenChanged;

    public enum State
    {
        Sad,
        Happy
    }

    void Start()
    {
        OnWaterChanged?.Invoke(water);
        OnOxygenChanged?.Invoke(oxygen);

        animator = cactusRenderer.GetComponent<Animator>();
        dayNightCycle = DayNightCycle.Instance;
        skinnedMeshRenderer = cactusRenderer.GetComponent<SkinnedMeshRenderer>();
        initCactus();
       
    }

    void initCactus()
    {
        petalsOpen = true;
        skinnedMeshRenderer.SetBlendShapeWeight(spikesIndex, 100);
        skinnedMeshRenderer.SetBlendShapeWeight(flowerIndex, 100);
    }

    private void Update()
    {
        oxygenLevel();
        waterLevel();
        
        CheckCactusState();
    }

    private void waterLevel()
    {
        waterTimer += Time.deltaTime;
        if (waterTimer >= interval && dayNightCycle.IsDay())
        {
            reduceWater();
        }
    }

    public void SunshineEffect()
    {

        oxygenLvl = 100 - oxygen; //the amount of oxygen the flower needs to increase in this specific day
        // Attempt to open petals with a 75% success rate
        if (UnityEngine.Random.value <= 0.75f)
        {
            animate(true, false);
           
            if (cactusState != State.Happy)
            {
                cactusState = State.Happy;
                setCactusFace(happyOffset);
            }
        }
        else
        {
            animate(false, true);

            if (cactusState != State.Sad)
            {
                cactusState = State.Sad;
                setCactusFace(sadOffset);
            }
        }
    }

    public void SunsetEffect()
    {

        // Attempt to close petals with a 75% success rate
        if (UnityEngine.Random.value <= 0.75f)
        {
            animate(false, false);

            if (cactusState != State.Happy)
            {
                cactusState = State.Happy;
                setCactusFace(happyOffset);
            }
        }
        else
        {
            animate(true, true);

            if (cactusState != State.Sad)
            {
                cactusState = State.Sad;
                setCactusFace(sadOffset);
            }
        }
    }

    public void animate(bool petals, bool spikes)
    {
        petalsOpen = petals;
        animator.SetBool("petalsOpen", petalsOpen);
        animator.SetBool("spikesOpen", spikes);
    }
    public void oxygenLevel()
    {
        //if day time
        if (dayNightCycle.IsDay())
        {
            if (petalsOpen) // and petals open
            {
                increaseOxygen();
            }
            else // and petals close
            {
                reduceOxygen(2);
            }   
        }

        //if petal open at night time
        if (petalsOpen && !dayNightCycle.IsDay())
        {
            reduceOxygen(10);
        }
    }

    private void increaseOxygen()
    {
        float oxygenToIncrease = oxygenLvl / (dayNightCycle.dayDuration / 2);
        float num = oxygen + (oxygenToIncrease * Time.deltaTime);
        oxygen = Mathf.Min(num, 100f);
        OnOxygenChanged?.Invoke(oxygen); // Trigger event
    }

    private void reduceOxygen(float rate)
    {
        oxygenTimer += Time.deltaTime;
        if (oxygenTimer >= interval)
        {
            oxygen -= rate;
            oxygen = Mathf.Max(oxygen, 0f);
            oxygenTimer = 0f;
            OnOxygenChanged?.Invoke(oxygen); // Trigger event
        }
    }

    private void reduceWater()
    {
        water -= UnityEngine.Random.Range(1f, 5f); 
        water = Mathf.Max(water, 0f);
        waterTimer = 0f;
        OnWaterChanged?.Invoke(water); // Trigger event
    }


    private void CheckCactusState()
    {
        if (water <= 0f || oxygen <= 0f)
        {
            cactusRenderer.material.color = Color.gray;
            setCactusFace(sadOffset);
            Time.timeScale = 0; //stop the scene
            cactusDieText.gameObject.SetActive(true);
        }
    }

    internal void WaterCactus()
    {
        waterEffect.Play();
        water *= 1.3f;
        if (water > 100)
        {
            water = 100;
        }
        OnWaterChanged?.Invoke(water); // Trigger event
    }

    internal void afterWater()
    {
        waterEffect.Stop();
        waterEffect.Clear();
    }

    internal void TogglePetals()
    {
        if (petalsOpen && !dayNightCycle.IsDay()) // petals open and night
        {
            petalsOpen = false;
            animator.SetBool("petalsOpen", petalsOpen);
            animator.SetBool("spikesOpen", false);

            if(cactusState != State.Happy)
            {
                cactusState= State.Happy;
                setCactusFace(happyOffset);
            }

           
        }
        else if(!petalsOpen && dayNightCycle.IsDay()) // petals close and day
        {
            oxygenLvl = 100 - oxygen;
            petalsOpen = true;
            animator.SetBool("petalsOpen", petalsOpen);
            animator.SetBool("spikesOpen", false);

            if (cactusState != State.Happy)
            {
                cactusState= State.Happy;
                setCactusFace(happyOffset);
            }
        }
    }

    public bool isPetalsOpen()
    {
        return petalsOpen;
    }

    void setCactusFace(Vector2 faceOffset)
    {
        skinnedMeshRenderer.material.SetTextureOffset("_BaseMap", faceOffset);
    }
}
