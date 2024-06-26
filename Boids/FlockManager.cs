using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlockManager : MonoBehaviour {
    public static FlockManager FM; //singleton pattern
    public Spawn spawner; //reference to the spawner
    public int numFish = 20;
    public int deadFish;
    public int actualFish;
    public int bornFish;
    public List<GameObject> allFish; //array of fish to access them later
    public List<GameObject> allFood; //array of food to access them later
    public Vector3 swimLimits = new Vector3(2.0f, 2.0f, 2.0f);
    public int healthyFoodConsumed;
    public int toxicFoodConsumed;
    public bool shouldRegenerateFish = false;

    [Header("Fish Settings")]
    [Range(1.0f, 3.0f)] public float minSize;
    [Range(1.0f, 3.0f)] public float maxSize;
    [Header("Boids Settings")]
    [Range(0.0f, 5.0f)] public float minSpeed;
    [Range(0.0f, 5.0f)] public float maxSpeed;
    [Range(0.1f, 2.0f)] public float maxNeighborDistance;
    [Range(0.1f, 2.0f)] public float separationDistance;
    [Range(1.0f, 5.0f)] public float rotationSpeed;
    [Header("Food Settings")]
    public float healthyFoodSpawnInterval  = 1.5f;
    public int nutritionValueHealthy = 20;
    public float toxicFoodSpawnInterval = 1.5f;
    public int nutritionValueToxic = 10;

    void Start() {
        allFish = new List<GameObject>();
        allFood = new List<GameObject>();
        //call to spawn the fishes
        allFish.AddRange(spawner.spawnFishes(numFish, swimLimits, this.transform.position));

        //initialize the actual number of fish
        actualFish = numFish;

        FM = this; //singleton pattern
    }

    void Update() {
        //call to spawn the food
        List<GameObject> newFood = spawner.spawnFood(swimLimits, this.transform.position, healthyFoodSpawnInterval, toxicFoodSpawnInterval);
        allFood.AddRange(newFood);

        if (actualFish == 0 && numFish > 0 && shouldRegenerateFish == true) {
            shouldRegenerateFish = false;
            allFish.AddRange(spawner.spawnFishes(numFish, swimLimits, this.transform.position));
            actualFish = numFish;
        }
    }

    //function to spawn the baby fishes
    public void spawnBabyFishes(Vector3 pos) {
        //instance of the fish
        GameObject fish = spawner.spawnBabyFish(pos);
        //call to the function to be a baby fish
        fish.GetComponent<Flock>().simulation.setBabyFish(true);
        //add the fish to the array
        allFish.Add(fish);
        //update the number of the actual fish
        actualFish++;
    }

    //function to clear the simulation
    public void ClearSimulation()
    {
        foreach (GameObject fish in allFish)
        {
            if (fish != null)
                Destroy(fish);
        }
        //clear the fish list
        allFish.Clear();
        actualFish = 0;
        bornFish = 0;
        deadFish = 0;

        foreach (GameObject food in allFood)
        {
            if (food != null)
                Destroy(food);
        }
        //clear the food list
        allFood.Clear();
        healthyFoodConsumed = 0;
        toxicFoodConsumed = 0;

    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue; 

        Gizmos.DrawWireCube(transform.position, swimLimits * 2.0f);

        if(FM != null){
            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(transform.position, FlockManager.FM.maxNeighborDistance);
        }
    }


}