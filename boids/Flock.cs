using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    float speed;
    bool turning = false;
    float fishSize;
    public int health = 100;
    public int energy = 100;
    float healthEnergyTimer = 0.0f; //timer for the health and energy decrease

    void Start() {
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
        fishSize = Random.Range(FlockManager.FM.minSize, FlockManager.FM.maxSize);
        transform.localScale = new Vector3(fishSize, fishSize, fishSize);
    }


    void Update() {
        //if the fish is out of the bounds of the tank, it will turn around
        Bounds b = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.swimLimits * 2.0f);
        if (!b.Contains(transform.position)) {
            turning = true;
        } else {
            turning = false;
        }

        //if the fish is turning, it will rotate towards the center of the tank
        if (turning) {
            Vector3 direction = FlockManager.FM.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                FlockManager.FM.rotationSpeed * Time.deltaTime);
        } 
        //if the fish is not turning, it will move forward
        else {
            //randomly change the speed and the direction of the fish
            if (Random.Range(0, 100) < 10) {
                //randomly change the speed of the fish
                speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
            }
            if (Random.Range(0, 100) < 10) {
                //randomly change the direction of the fish
                ApplyRules();
            }
        }
        this.transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);

        //decrease the health and energy of the fish every second
        healthEnergyTimer += Time.deltaTime;
        if(healthEnergyTimer >= 1.0f) {
            health -= 1;
            energy -= 1;
            healthEnergyTimer = 0.0f;
        }

        //search for food if the energy of the fish is less than 50
        if(energy < 80) {
            GameObject closestFood = FindClosestFood();
            if (closestFood != null){
                Vector3 directionToFood = (closestFood.transform.position - transform.position).normalized;
                //we can adjust the strength of the attraction to food to control how much affect it has on the fish
                float foodAttractionStrength = 0.1f;
                Vector3 newDirection = Vector3.Lerp(transform.forward, directionToFood, foodAttractionStrength).normalized;
                transform.rotation = Quaternion.LookRotation(newDirection);
                this.transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
            }
        }

    }

    //if the fish collides with the food, it will consume the food
    void OnTriggerEnter(Collider other) {
        Food foodItem = other.gameObject.GetComponent<Food>();
        if (foodItem != null) {
            foodItem.Consume(this);
            Destroy(other.gameObject); //destroy the food after the fish consumes it
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

    //flocking algorithm rules
    void ApplyRules() {
        GameObject[] gos;
        gos = FlockManager.FM.allFish;
        Vector3 vCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        float gSpeed = 0.01f;
        float mDistance;
        int groupSize = 0;

        //calculate the center of the group and the distance between the fishes
        foreach (GameObject go in gos) {
            if (go != this.gameObject) {
                mDistance = Vector3.Distance(go.transform.position, this.transform.position);
                //if the distance between the fishes is less than the neighbour distance, the fish will be part of the group
                if (mDistance <= FlockManager.FM.neighbourDistance) {
                    vCentre += go.transform.position;
                    groupSize++;
                    //if the distance between the fishes is less than 1, the fish will avoid the other fish
                    if (mDistance < 1.0f) {
                        vAvoid = vAvoid + (this.transform.position - go.transform.position);
                    }
                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        //if the fish is part of a group, it will move towards the center of the group
        if (groupSize > 0) {
            vCentre = vCentre / groupSize;
            speed = gSpeed / groupSize;
            if (speed > FlockManager.FM.maxSpeed) {
                speed = FlockManager.FM.maxSpeed;
            }
            Vector3 direction = (vCentre + vAvoid) - transform.position;
            if (direction != Vector3.zero) {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    FlockManager.FM.rotationSpeed * Time.deltaTime);
            }
        }
    }

    //getter and setter methods for external classes
    public void setSpeed(float newSpeed) {
        speed = newSpeed;
    }
    public float getSpeed() {
        return speed;
    }

    public void setEnergy(int newEnergy) {
        energy = newEnergy;
    }
    public int getEnergy() {
        return energy;
    }

    public void setHealth(int newHealth) {
        health = newHealth;
    }
    public int getHealth() {
        return health;
    }
}