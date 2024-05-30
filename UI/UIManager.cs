using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TMP_InputField numberTotalOfFishInputTMP;
    public TMP_InputField numberActualOfFishInputTMP;
    public TMP_InputField numberOfDeadFishInputTMP;
    public TMP_InputField numberOfNewBornFishInputTMP;
    public TMP_InputField numberOfHealthyFoodConsumedInputTMP;
    public TMP_InputField numberOfToxicFoodConsumedInputTMP;
    public GameObject graphScreen;
    public Button showGraphButton;
    public Button showSimulationButton;
    public TextMeshProUGUI actualFishText;
    public TextMeshProUGUI deadFishText;
    public TextMeshProUGUI bornFishText;
    //add more UI Components as needed


    private void Start()
    {
        //start with the simulation disabled
        flockManager.enabled = false;

        //disable the input fields. Use them to show data of the simulation.
        numberTotalOfFishInputTMP.interactable = false;
        numberActualOfFishInputTMP.interactable = false;
        numberOfDeadFishInputTMP.interactable = false;
        numberOfNewBornFishInputTMP.interactable = false;
        numberOfHealthyFoodConsumedInputTMP.interactable = false;
        numberOfToxicFoodConsumedInputTMP.interactable = false;

        //show graph button is disabled at the beginning
        showGraphButton.interactable = false;
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
        //we put the total number of fish at the beginning of the simulation
        numberTotalOfFishInputTMP.text = flockManager.numFish.ToString();

        //show graph button is enabled
        showGraphButton.interactable = true;
    }

    public void ShowGraph()
    {
        //show the graph
        graphScreen.gameObject.SetActive(true);
        showGraphButton.gameObject.SetActive(false);
        showSimulationButton.gameObject.SetActive(true);

        //change the text colors of the text input fields
        numberActualOfFishInputTMP.textComponent.color = HexToColor("517CE5");
        numberOfDeadFishInputTMP.textComponent.color = HexToColor("EA8383");
        numberOfNewBornFishInputTMP.textComponent.color = HexToColor("74E07F");

        //change the text color of the text components
        actualFishText.color = HexToColor("517CE5");
        deadFishText.color = HexToColor("EA8383");
        bornFishText.color = HexToColor("74E07F");

    }

    public void ShowSimulation()
    {
        //show the simulation
        graphScreen.gameObject.SetActive(false);
        showSimulationButton.gameObject.SetActive(false);
        showGraphButton.gameObject.SetActive(true);

        //change the text colors of the text input fields
        numberActualOfFishInputTMP.textComponent.color = Color.white;
        numberOfDeadFishInputTMP.textComponent.color = Color.white;
        numberOfNewBornFishInputTMP.textComponent.color = Color.white;

        //change the text color of the text components
        actualFishText.color = Color.white;
        deadFishText.color = Color.white;
        bornFishText.color = Color.white;
    }

    public void Update() {
        //update the text of the input fields
        numberActualOfFishInputTMP.text = flockManager.actualFish.ToString();
        numberOfNewBornFishInputTMP.text = flockManager.bornFish.ToString();
        numberOfDeadFishInputTMP.text = flockManager.deadFish.ToString();
        numberOfHealthyFoodConsumedInputTMP.text = flockManager.healthyFoodConsumed.ToString();
        numberOfToxicFoodConsumedInputTMP.text = flockManager.toxicFoodConsumed.ToString();
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
        flockManager.maxNeighborDistance = value;
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

    Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color(r / 255f, g / 255f, b / 255f);
    }

}
