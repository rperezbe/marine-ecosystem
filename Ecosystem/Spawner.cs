using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    //references to all the prefabs
    public GameObject fishPrefab;
    public GameObject healthyFoodPrefab;
    public GameObject toxicFoodPrefab;

    //timer for the food spawn
    private float healthyFoodTimer  = 0.0f; 
    private float toxicFoodTimer  = 0.0f;

    //function to spawn the fish
    public GameObject[] spawnFishes(int numFish, Vector3 swimLimits, Vector3 origin) {
        GameObject[] allFish = new GameObject[numFish];
        for (int i = 0; i < numFish; ++i) {
            Vector3 pos = origin + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
            
            //instance of the fish
            GameObject fish = Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i] = fish;
        }
        return allFish;
    }

    //function to spawn the baby fishes
    public GameObject spawnBabyFish(Vector3 pos) {
        //instance of the fish
        GameObject fish = Instantiate(fishPrefab, pos, Quaternion.identity);
        return fish;
    }

    //function to spawn the food
    public void spawnFood(Vector3 swimLimits, Vector3 origin, float healthyFoodInterval, float toxicFoodInterval) {
        //update the timer
        healthyFoodTimer += Time.deltaTime;
        toxicFoodTimer += Time.deltaTime;

        //if the timer is greater than the spawn interval, spawn a new healthy food
        if (healthyFoodTimer >= healthyFoodInterval) {
            healthyFoodTimer = 0.0f;
            Vector3 foodPosition = origin + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
            Instantiate(healthyFoodPrefab, foodPosition, Quaternion.identity);
        }

        // Si el temporizador supera el intervalo, generar nueva comida tóxica.
        if (toxicFoodTimer >= toxicFoodInterval) {
            toxicFoodTimer = 0.0f;
            //create food on the random position
            Vector3 foodPosition = origin + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
            Instantiate(toxicFoodPrefab, foodPosition, Quaternion.identity);
        }
    }
}
