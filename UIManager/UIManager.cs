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
    public Slider neighbourDistanceSlider;
    public Slider numberOfFishInput;
    public Slider nutritionValueHealthySlider;
    public Slider nutritionValueToxicSlider;
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
        //don't show the button anymore
        startSimulationButton.gameObject.SetActive(false);

        //don't allow to change anything else once the simulation has started
        /*frequencySliderHealthy.interactable = false;
        frequencySliderToxic.interactable = false;
        neighbourDistanceSlider.interactable = false;*/
        numberOfFishInput.interactable = false;
        nutritionValueHealthySlider.interactable = false;
        nutritionValueToxicSlider.interactable = false;
    }

    //change the nutrition value of the healthy food
    public void ChangeNutritionValueHealthy(float value)
    {
        flockManager.nutritionValueHealthy = (int)value;
    }

    //change the nutrition value of the toxic food
    public void ChangeNutritionValueToxic(float value)
    {
        flockManager.nutritionValueToxic = (int)value;
    }

    //method to change the number of fish
    public void ChangeNumberOfFish(float value)
    {
        flockManager.numFish = (int)value;
    }

    //method to change the distance between the neighbours
    public void ChangeNeighbourDistance(float value)
    {
        flockManager.neighbourDistance = value;
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