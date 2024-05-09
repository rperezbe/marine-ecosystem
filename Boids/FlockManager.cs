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
    public GameObject[] allFish; //array of fish to access them later
    public Vector3 swimLimits = new Vector3(2.0f, 2.0f, 2.0f);
    public int healthyFoodConsumed;
    public int toxicFoodConsumed;

    [Header("Fish Settings")]
    [Range(1.0f, 3.0f)] public float minSize;
    [Range(1.0f, 3.0f)] public float maxSize;
    [Header("Boids Settings")]
    [Range(0.0f, 5.0f)] public float minSpeed;
    [Range(0.0f, 5.0f)] public float maxSpeed;
    [Range(1.0f, 10.0f)] public float neighbourDistance;
    [Range(1.0f, 5.0f)] public float rotationSpeed;
    [Header("Food Settings")]
    public float healthyFoodSpawnInterval  = 0.5f;
    public int nutritionValueHealthy = 20;
    public float toxicFoodSpawnInterval = 0.5f;
    public int nutritionValueToxic = 10;

    void Start() {
        allFish = new GameObject[numFish];

        //call to spawn the fishes
        allFish = spawner.spawnFishes(numFish, swimLimits, this.transform.position);

        //initialize the actual number of fish
        actualFish = numFish;

        FM = this; //singleton pattern
    }

    void Update() {
        //call to spawn the food
        spawner.spawnFood(swimLimits, this.transform.position, healthyFoodSpawnInterval, toxicFoodSpawnInterval);
    }

    //function to spawn the baby fishes
    public void spawnBabyFishes(Vector3 pos) {
        //instance of the fish
        GameObject fish = spawner.spawnBabyFish(pos);
        //call to the function to be a baby fish
        fish.GetComponent<Flock>().simulation.setBabyFish(true);
        //add the fish to the array
        int index = allFish.Length;
        allFish[index-1] = fish;
        //update the number of the actual fish
        actualFish++;
    }
}