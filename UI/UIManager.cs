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
    public TextMeshProUGUI healthyFoodText;
    public TextMeshProUGUI toxicFoodText;
    //instantiate the backgrounds
    public Image background1;
    public Image background2;
    public Image background3;
    //instantiate the button backgrounds
    public Button buttonBackground1;
    public Button buttonBackground2;
    public Button buttonBackground3;
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

        numberOfFishInput.interactable = false;
        nutritionValueHealthySlider.interactable = false;
        nutritionValueToxicSlider.interactable = false;
        //we put the total number of fish at the beginning of the simulation
        numberTotalOfFishInputTMP.text = flockManager.numFish.ToString();

        //hide the button backgrounds
        buttonBackground1.gameObject.SetActive(false);
        buttonBackground2.gameObject.SetActive(false);
        buttonBackground3.gameObject.SetActive(false);

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
        numberActualOfFishInputTMP.textComponent.color = HexToColor("7A99E5");
        numberOfDeadFishInputTMP.textComponent.color = HexToColor("000000");
        numberOfNewBornFishInputTMP.textComponent.color = HexToColor("DECF7B");
        numberOfHealthyFoodConsumedInputTMP.textComponent.color = HexToColor("74E07F");
        numberOfToxicFoodConsumedInputTMP.textComponent.color = HexToColor("EA8383");

        //change the text color of the text components
        actualFishText.color = HexToColor("7A99E5");
        deadFishText.color = HexToColor("000000");
        bornFishText.color = HexToColor("DECF7B");
        healthyFoodText.color = HexToColor("74E07F");
        toxicFoodText.color = HexToColor("EA8383");

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
        numberOfHealthyFoodConsumedInputTMP.textComponent.color = Color.white;
        numberOfToxicFoodConsumedInputTMP.textComponent.color = Color.white;

        //change the text color of the text components
        actualFishText.color = Color.white;
        deadFishText.color = Color.white;
        bornFishText.color = Color.white;
        healthyFoodText.color = Color.white;
        toxicFoodText.color = Color.white;
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

    //method to change the min separation distance
    public void ChangeMinSeparation(float value)
    {
        flockManager.separationDistance = value;
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
    
    public void OnBackground1Clicked()
    {
        //show the background image
        background1.gameObject.SetActive(true);
        //hide the other background images
        background2.gameObject.SetActive(false);
        background3.gameObject.SetActive(false);

        //change the opacity of the button backgrounds
        buttonBackground1.GetComponent<Image>().color = new Color(1, 1, 1, 1); //full opacity
        buttonBackground2.GetComponent<Image>().color = new Color(1, 1, 1, 0.588f); //minor opacity
        buttonBackground3.GetComponent<Image>().color = new Color(1, 1, 1, 0.588f); ////minor opacity
    }

    public void OnBackground2Clicked()
    {
        background1.gameObject.SetActive(false);
        background2.gameObject.SetActive(true);
        background3.gameObject.SetActive(false);

        buttonBackground1.GetComponent<Image>().color = new Color(1, 1, 1, 0.588f);
        buttonBackground2.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        buttonBackground3.GetComponent<Image>().color = new Color(1, 1, 1, 0.588f);
    }

    public void OnBackground3Clicked()
    {
        background1.gameObject.SetActive(false);
        background2.gameObject.SetActive(false);
        background3.gameObject.SetActive(true);

        buttonBackground1.GetComponent<Image>().color = new Color(1, 1, 1, 0.588f);
        buttonBackground2.GetComponent<Image>().color = new Color(1, 1, 1, 0.588f);
        buttonBackground3.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

}
