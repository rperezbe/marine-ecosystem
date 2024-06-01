using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphController : MonoBehaviour
{
    public Image fishBar;
    public Image fishBornBar;
    public Image fishDeadBar;
    public Image lineGraph;
    public TextMeshProUGUI maxNumber;
    public TextMeshProUGUI mediumNumber;

    private Texture2D graphTexture;
    private List<float> fishCounts = new List<float>();
    private List<float> fishBornCounts = new List<float>();
    private List<float> fishDeadCounts = new List<float>();
    private float maxFish = 100;

    private float updateInterval = 1.0f;
    private float timeSinceLastUpdate = 0;

    void Start()
    {
        graphTexture = new Texture2D(150, 150);
        graphTexture.filterMode = FilterMode.Point;
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
        //assign the maxNumber text value to the maxFish
        maxNumber.text = maxFish.ToString();
        //assign the mediumNumber text value to the half of the maxFish
        mediumNumber.text = (maxFish / 2).ToString();

        if (FlockManager.FM == null)
        {
            return;
        }

        //update the graph with the actual number of fish
        float currentFish = FlockManager.FM.actualFish;
        float normalizedFish = currentFish / maxFish;
        fishBar.fillAmount = normalizedFish;
        UpdateDataList(fishCounts, normalizedFish);

        float currentFishBorn = FlockManager.FM.bornFish;
        float normalizedFishBorn = currentFishBorn / maxFish;
        fishBornBar.fillAmount = normalizedFishBorn;
        UpdateDataList(fishBornCounts, normalizedFishBorn);

        float currentFishDead = FlockManager.FM.deadFish;
        float normalizedFishDead = currentFishDead / maxFish;
        fishDeadBar.fillAmount = normalizedFishDead;
        UpdateDataList(fishDeadCounts, normalizedFishDead);

        if (currentFish > maxFish || currentFishBorn > maxFish || currentFishDead > maxFish)
        {
            maxFish *= 2;  //adjust max scale as needed
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
            DrawLine(x0, fishCounts[i - 1] * height, x1, fishCounts[i] * height, HexToColor("517CE5"));
            DrawLine(x0, fishBornCounts[i - 1] * height, x1, fishBornCounts[i] * height, HexToColor("74E07F"));
            DrawLine(x0, fishDeadCounts[i - 1] * height, x1, fishDeadCounts[i] * height, HexToColor("EA8383"));
        }
        graphTexture.Apply();
    }

    void DrawLine(int x0, float y0, int x1, float y1, Color color)
    {
        //implement a simple line drawing algorithm (Bresenham's line algorithm)
        int dy = (int)(y1 - y0);
        int dx = x1 - x0;
        int step = dy < 0 ? -1 : 1;
        dy = Mathf.Abs(dy) * 2;
        dx *= 2;

        float fraction = 0;

        graphTexture.SetPixel(x0, (int)y0, color);

        while (x0 != x1)
        {
            if (fraction >= 0)
            {
                y0 += step;
                fraction -= dx;
            }
            x0++;
            fraction += dy;
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
