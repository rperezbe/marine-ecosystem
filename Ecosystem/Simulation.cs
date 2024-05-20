using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour {
    public int health = 100;
    public int energy = 70; //we start with 70% energy 
    float healthEnergyTimer = 0.0f; //timer for the health and energy decrease
    public float searchFoodCooldown = 3.0f; //cooldown for the fish to search for food, starts in 3 seconds
    public bool isBabyFish = false;
    public GameObject closestFood = null;  // Almacena la referencia al alimento más cercano


    public Flock associatedFlock;

    void Start() {
        //at the moment, anything
    }

    void Update() {
        //decrease the health and energy of the fish every second
        healthEnergyTimer += Time.deltaTime;
        if(healthEnergyTimer >= 1.0f) {
            //if energy is 0, the health will decrease faster. 
            //if not, the energy and health will decrease one unit
            if(energy == 0) {
                health -= 5;
            }else{
                health -= 1;
                energy -= 1;
            }
            
            //clamp the health and energy between 0 and 100
            health = Mathf.Clamp(health, 0, 100);
            energy = Mathf.Clamp(energy, 0, 100);

            healthEnergyTimer = 0.0f;
        }

        //reduce the cooldown of the fish to search for food
        if(searchFoodCooldown > 0) {
            searchFoodCooldown -= Time.deltaTime;
        }

        //search for food if the energy and the cooldown are ok
        if (energy < Random.Range(70, 81) && searchFoodCooldown <= 0) {
            closestFood = FindClosestFood();
            if (closestFood != null){
                Vector3 directionToFood = (closestFood.transform.position - transform.position).normalized;
                //we can adjust the strength of the attraction to food to control how much affect it has on the fish
                float distanceToFood = Vector3.Distance(transform.position, closestFood.transform.position);
                //the max distance at which the food will have full attraction strength
                float maxAttractionDistance = 1f;
                //we use clamp01 to make sure the value is between 0 and 1
                float foodAttractionStrength = Mathf.Clamp01(1f - distanceToFood / maxAttractionDistance); 
                //we use Lerp to smoothly change the direction of the fish towards the food 
                Vector3 newDirection = Vector3.Lerp(transform.forward, directionToFood, foodAttractionStrength).normalized;
                transform.rotation = Quaternion.LookRotation(newDirection);
                this.transform.Translate(0.0f, 0.0f, associatedFlock.speed * Time.deltaTime);
            }
        }

        //influence speed from min speed to max speed depending on the energy
        associatedFlock.speed = Mathf.Lerp(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed, (float)energy / 100);

        //if health is less than 0, the fish will die
        if(health <= 0) {
            Destroy(this.gameObject);
            
            //update the counter of the dead fish
            FlockManager.FM.deadFish++;
            //update the counter of the actual fish
            FlockManager.FM.actualFish--;
        }

        //if energy is above 95, the fish will reproduce
        if(energy >= 95) {
            //reduce the energy of the parent fish due to the reproduction
            energy = 50;
            //spawn the baby fish in the same position of the parent fish
            FlockManager.FM.spawnBabyFishes(transform.position);
            //update the number of the born fish
            FlockManager.FM.bornFish++;
        }
    }

    //if the fish collides with the food, it will consume the food
    void OnTriggerEnter(Collider other) {
        Food foodItem = other.gameObject.GetComponent<Food>();
        if (foodItem != null) {
            foodItem.Consume(this);
            Destroy(other.gameObject); //destroy the food after the fish consumes it
            
            //increase the counters of the consumed food
            if (foodItem.name.Contains("Toxic")) {
                FlockManager.FM.toxicFoodConsumed++;
            } else {
                FlockManager.FM.healthyFoodConsumed++;
            }
        }
    }
    
    //find the closest food to the fish
    GameObject FindClosestFood() {
        Food[] foods = FindObjectsOfType<Food>(); //find all the food in the tank
        GameObject closest = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Food food in foods) {
            Vector3 directionToTarget = food.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                closest = food.gameObject;
            }
        }
        return closest;
    }

    void OnDrawGizmos() {
        if (closestFood != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, closestFood.transform.position);
        }
    }

    /*SPEED IS NOW WORKING WITH ENERGY 
    //getter and setter methods for external classes
    public void setSpeed(float newSpeed) {
        //clamp the speed between the min and max speed
        //speed = Mathf.Clamp(newSpeed, FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
    }
    public float getSpeed() {
        return speed;
    }*/

    public void setBabyFish(bool isBabyFish) {
        //set the flag baby fish to true
        this.isBabyFish = isBabyFish;
        if(isBabyFish) {
            //set the energy of the baby fish to 50
            energy = 50;
            //set the health of the baby fish to 50
            health = 50;
            //set the size of the baby fish to min
            associatedFlock.fishSize = FlockManager.FM.minSize;
            transform.localScale = new Vector3(associatedFlock.fishSize, associatedFlock.fishSize, associatedFlock.fishSize);
        }
    }

    public void setEnergy(int newEnergy) {
        //clamp the energy between 0 and 100
        energy = Mathf.Clamp(newEnergy, 0, 100);
    }

    public int getEnergy() {
        return energy;
    }

    public void setHealth(int newHealth) {
        //clamp the health between 0 and 100
        health = Mathf.Clamp(newHealth, 0, 100);
    }
    public int getHealth() {
        return health;
    }
}