using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlockManager : MonoBehaviour {
    public static FlockManager FM;
    public GameObject fishPrefab;
    public GameObject healthyFoodPrefab;
    public GameObject toxicFoodPrefab;
    private float timer = 0.0f; //timer for the food spawn
    public int numFish = 40;
    public GameObject[] allFish; //array of fish to access them later
    public Vector3 swimLimits = new Vector3(2.0f, 2.0f, 2.0f);
    public Vector3 goalPos = Vector3.zero;

    [Header("Fish Settings")]
    [Range(1.0f, 5.0f)] public float minSize;
    [Range(1.0f, 5.0f)] public float maxSize;
    [Header("Boids Settings")]
    [Range(0.0f, 5.0f)] public float minSpeed;
    [Range(0.0f, 5.0f)] public float maxSpeed;
    [Range(1.0f, 10.0f)] public float neighbourDistance;
    [Range(1.0f, 5.0f)] public float rotationSpeed;
    [Header("Food Settings")]
    public float spawnInterval = 5.0f;

    void Start() {

        allFish = new GameObject[numFish];

        //call to spawn the fishes
        spawnFishes();

        FM = this; //singleton pattern
        goalPos = this.transform.position;
    }

    void Update() {
        //call to spawn the food
        spawnFood();
    }

    //function to spawn the fish
    void spawnFishes() {
        for (int i = 0; i < numFish; ++i) {
            Vector3 pos = this.transform.position + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));

            //instance of the fish
            GameObject fish = Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i] = fish;
        }
    }

    //function to spawn the food
    void spawnFood() {
        //decide randomly what type of food to spawn
        GameObject foodPrefab = Random.Range(0, 2) == 0 ? healthyFoodPrefab : toxicFoodPrefab;
        timer += Time.deltaTime;
        //if the timer is greater than the spawn interval, spawn a new food
        if (timer >= spawnInterval) {
            //reset the timer
            timer = 0.0f;
            //create food on the random position
            Vector3 foodPosition = this.transform.position + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
            Instantiate(foodPrefab, foodPosition, Quaternion.identity);
            //goalpos is the position of the food
            goalPos = foodPosition;
        }
    }
}