using UnityEngine;
using UnityEngine.UI;

public class GraphController : MonoBehaviour
{
    public Image fishBar;
    private float maxFish = 500; //we put this maximum to normalize the value, we can change it

    void Update()
    {
        UpdateGraph();
    }

    void UpdateGraph()
    {
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
    }

}