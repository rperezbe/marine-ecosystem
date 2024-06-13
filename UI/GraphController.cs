using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphController : MonoBehaviour
{
    public Image healthyFoodConsumedBar;
    public Image toxicFoodConsumedBar;
    public Image lineGraph;
    public TextMeshProUGUI maxNumberBar;
    public TextMeshProUGUI mediumNumberBar;
    public TextMeshProUGUI maxNumberLine;
    public TextMeshProUGUI mediumNumberLine;

    private Texture2D graphTexture;
    private List<float> fishCounts = new List<float>();
    private List<float> fishBornCounts = new List<float>();
    private List<float> fishDeadCounts = new List<float>();
    private float maxFood = 100;
    private float maxFish = 50;

    private float updateInterval = 1.0f;
    private float timeSinceLastUpdate = 0;

    void Start()
    {
        graphTexture = new Texture2D(280, 100);
        graphTexture.filterMode = FilterMode.Point; //important to avoid blurring
        lineGraph.sprite = Sprite.Create(graphTexture, new Rect(0, 0, graphTexture.width, graphTexture.height), new Vector2(0.5f, 0.5f), 100.0f, 0, SpriteMeshType.FullRect);
        ClearGraph();
    }


    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= updateInterval)
        {
            UpdateGraph();
            timeSinceLastUpdate = 0;
        }
    }

    void UpdateGraph()
    {
        //assign the maxNumber text value to the maxFood
        maxNumberBar.text = maxFood.ToString();
        //assign the mediumNumber text value to the half of the maxFood
        mediumNumberBar.text = (maxFood / 2).ToString();

        //assign the maxNumber text value to the maxFish
        maxNumberLine.text = maxFish.ToString();
        //assign the mediumNumber text value to the half of the maxFish
        mediumNumberLine.text = (maxFish / 2).ToString();

        if (FlockManager.FM == null)
        {
            return;
        }

        //update the graph with the actual number of consumed food
        float currentHealthyFood = FlockManager.FM.healthyFoodConsumed;
        float normalizedHealthyFood = currentHealthyFood / maxFood;
        healthyFoodConsumedBar.fillAmount = normalizedHealthyFood;
        
        //update the graph with the actual number of consumed toxic food
        float currentToxicFood = FlockManager.FM.toxicFoodConsumed;
        float normalizedToxicFood = currentToxicFood / maxFood;
        toxicFoodConsumedBar.fillAmount = normalizedToxicFood;
        
        if (currentHealthyFood > maxFood || currentToxicFood > maxFood)
        {
            maxFood *= 2;  //adjust max scale of food as needed
        }

        float currentFish = FlockManager.FM.actualFish;
        float normalizedFish = currentFish / maxFish;
        // fishBar.fillAmount = normalizedFish;
        UpdateDataList(fishCounts, normalizedFish);

        float currentFishBorn = FlockManager.FM.bornFish;
        float normalizedFishBorn = currentFishBorn / maxFish;
        // fishBornBar.fillAmount = normalizedFishBorn;
        UpdateDataList(fishBornCounts, normalizedFishBorn);

        float currentFishDead = FlockManager.FM.deadFish;
        float normalizedFishDead = currentFishDead / maxFish;
        //fishDeadBar.fillAmount = normalizedFishDead;
        UpdateDataList(fishDeadCounts, normalizedFishDead);

        if (currentFish > maxFish || currentFishBorn > maxFish || currentFishDead > maxFish)
        {
            maxFish *= 2;  //adjust max scale of fishes as needed
        }

        DrawLineGraph();

    }

    void ClearGraph()
    {
        for (int x = 0; x < graphTexture.width; x++)
        {
            for (int y = 0; y < graphTexture.height; y++)
            {
                graphTexture.SetPixel(x, y, Color.clear);
            }
        }
        graphTexture.Apply();
    }

    void DrawLineGraph()
    {
        ClearGraph(); //clear the previous frame

        //ensure you scale x coordinates to fit the graph width
        int width = graphTexture.width;
        int height = graphTexture.height;
        int count = fishCounts.Count;

        for (int i = 1; i < count; i++)
        {
            int x0 = (i - 1) * width / count;
            int x1 = i * width / count;
            BresenhamError(x0, fishCounts[i - 1] * height, x1, fishCounts[i] * height, HexToColor("7A99E5"));
            BresenhamError(x0, fishBornCounts[i - 1] * height, x1, fishBornCounts[i] * height, HexToColor("DECF7B"));
            BresenhamError(x0, fishDeadCounts[i - 1] * height, x1, fishDeadCounts[i] * height, HexToColor("000000"));
        }
        graphTexture.Apply();
    }

    void BresenhamError(int x0, float y0, int x1, float y1, Color color)
    {
        //bresenham's line algorithm
        //difference between the initial and final y coordinates
        int dy = (int)(y1 - y0);
        //difference between the initial and final x coordinates
        int dx = x1 - x0;
        //evaluate if the line is going up or down
        int step = dy < 0 ? -1 : 1;
        //duplicate the absolute value of the difference between the initial and final y coordinates
        dy = Mathf.Abs(dy) * 2;
        //duplicate the difference between the initial and final x coordinates
        dx *= 2;

        //initialize the error
        float fraction = 0;

        //set the pixel in the initial coordinates
        graphTexture.SetPixel(x0, (int)y0, color);

        //while until the initial x coordinate is equal to the final x coordinate
        while (x0 != x1)
        {
            //evaluate if we need to increment or decrement the y coordinate
            if (fraction >= 0)
            {
                //increment or decrement the y coordinate
                y0 += step;
                //adjust the error
                fraction -= dx;
            }
            //if not, increment the x coordinate advancing to the final x coordinate
            x0++;
            //adjust the error
            fraction += dy;
            //set the pixel in the calculated coordinates
            graphTexture.SetPixel(x0, (int)y0, color);
        }
    }

    void UpdateDataList(List<float> dataList, float newValue)
    {
        const int maxPoints = 100;  //max number of points to display on the graph
        if (dataList.Count >= maxPoints)
        {
            dataList.RemoveAt(0);  //remove the oldest data point
        }
        dataList.Add(newValue);  //ddd the new data point at the end
    }

    Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color(r / 255f, g / 255f, b / 255f);
    }
}
