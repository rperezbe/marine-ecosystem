using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphController : MonoBehaviour
{
    public Image fishBar;
    public Image fishBornBar;
    public Image fishDeadBar;
    public TextMeshProUGUI maxNumber;
    public TextMeshProUGUI mediumNumber;
    
    private float maxFish = 100; //we put this maximum to normalize the value, we can change it
    void Update()
    {
        UpdateGraph();
    }

    void UpdateGraph()
    {
        //assign the maxNumber text value to the maxFish
        maxNumber.text = maxFish.ToString();
        //assign the mediumNumber text value to the half of the maxFish
        mediumNumber.text = (maxFish / 2).ToString();

        //if the FlockManager is not initialized, return
        if(FlockManager.FM == null)
        {
            return;
        }

        //update the graph with the actual number of fish
        float currentFish = FlockManager.FM.actualFish;
        float normalizedValue = currentFish / maxFish;
        
        //update the fill amount of the fish bar
        fishBar.fillAmount = normalizedValue;

        //update the graph with the number of fish born
        float currentFishBorn = FlockManager.FM.bornFish;
        float normalizedValueBorn = currentFishBorn / maxFish;

        //update the fill amount of the fish born bar
        fishBornBar.fillAmount = normalizedValueBorn;

        //update the graph with the number of fish dead
        float currentFishDead = FlockManager.FM.deadFish;
        float normalizedValueDead = currentFishDead / maxFish;

        //update the fill amount of the fish dead bar
        fishDeadBar.fillAmount = normalizedValueDead;

        //see if any value is greater than the maxFish, then update the maxFish to the double of the current value
        if(currentFish > maxFish || currentFishBorn > maxFish || currentFishDead > maxFish)
        {
            maxFish = maxFish * 2;
        }
    }

}