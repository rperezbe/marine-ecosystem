using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public FlockManager flockManager;
    public Button startSimulationButton;
    public Slider frequencySliderHealthy;
    public Slider frequencySliderToxic;
    //add more UI Components as needed

    private void Start()
    {
        //start with the simulation disabled
        flockManager.enabled = false;

    }

    //method to start the simulation (asigned to the event start simulation button)
    public void StartSimulation()
    {
        flockManager.enabled = true;
    }

    //method to change the frequency of the food spawn
    public void ChangeFoodSpawnFrequencyHealthy(float value)
    {
        flockManager.healthyFoodSpawnInterval = value;
    }

    //method to change the frequency of the food spawn toxic
    public void ChangeFoodSpawnFrequencyToxic(float value)
    {
        flockManager.toxicFoodSpawnInterval = value;
    }

}
