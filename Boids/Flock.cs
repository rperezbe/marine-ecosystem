using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    public float speed;
    bool turning = false;
    public float fishSize;
    public Simulation simulation;

    void Start() {        
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);

        //don't affect the size of the baby fish
        if (!simulation.isBabyFish) {
            fishSize = Random.Range(FlockManager.FM.minSize, FlockManager.FM.maxSize);
            transform.localScale = new Vector3(fishSize, fishSize, fishSize);
        }
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
            //if (Random.Range(0, 100) < 10) {
                //randomly change the speed of the fish
                //speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
            //}
            if (Random.Range(0, 100) < 10) {
                //randomly change the direction of the fish
                ApplyRules();
            }
        }
        this.transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
    }

    //flocking algorithm rules
    void ApplyRules() {
        List<GameObject> neighbors = new List<GameObject>();
        
        //find the neighbors of the fish within the neighbour distance set in the FlockManager
        foreach (GameObject go in FlockManager.FM.allFish) {
            if (go != null && go != gameObject && Vector3.Distance(transform.position, go.transform.position) <= FlockManager.FM.neighbourDistance) {
                neighbors.Add(go);
            }
        }

        //calculate the separation, cohesion and alignment forces
        Vector3 separation = CalculateSeparation(neighbors, 1.0f); //you can adjust the separation distance
        Vector3 cohesion = CalculateCohesion(neighbors);
        Vector3 alignment = CalculateAlignment(neighbors);

        //combine the forces to get the move direction
        Vector3 moveDirection = separation + cohesion + alignment;

        //rotate the fish towards the move direction
        if (moveDirection != Vector3.zero) {
            Quaternion desiredRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, FlockManager.FM.rotationSpeed * Time.deltaTime);
        }
        
        //move the fish forward in the move direction
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


    private Vector3 CalculateSeparation(List<GameObject> neighbors, float minDistance) {
        Vector3 separationSum = Vector3.zero;
        foreach (GameObject neighbor in neighbors) {
            Vector3 diff = transform.position - neighbor.transform.position;
            float dist = diff.magnitude;
            if (dist < minDistance && dist > 0) {
                separationSum += diff.normalized / dist; //normalized diff vector divided by the distance
            }
        }
        return separationSum;
    }

    private Vector3 CalculateCohesion(List<GameObject> neighbors) {
        Vector3 centerMass = Vector3.zero;
        if (neighbors.Count == 0) return centerMass;

        foreach (GameObject neighbor in neighbors) {
            centerMass += neighbor.transform.position;
        }
        centerMass /= neighbors.Count;
        return (centerMass - transform.position).normalized; //direction from the fish to the center of mass
    }

    private Vector3 CalculateAlignment(List<GameObject> neighbors) {
        Vector3 averageVelocity = Vector3.zero;
        if (neighbors.Count == 0) return averageVelocity;

        foreach (GameObject neighbor in neighbors) {
            Flock flock = neighbor.GetComponent<Flock>();
            if (flock != null) averageVelocity += flock.speed * neighbor.transform.forward;
        }
        averageVelocity /= neighbors.Count;
        return averageVelocity.normalized;
    }

}