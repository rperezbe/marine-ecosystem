using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthyFood : Food
{
    void Start()
    {
        //we can use the singleton pattern of the FlockManager to get the nutrition value of the food
        //if we want to change the nutrition value during the simulation, we can do it on the consume method
        nutritionValue = FlockManager.FM.nutritionValueHealthy;
    }

    public override void Consume(Simulation consumer) 
    {
        //increase the energy of the fish by the nutrition value of the food
        //consumer.setEnergy(consumer.getEnergy() + nutritionValue);

        //increase the energy of the fish
        consumer.setEnergy(consumer.getEnergy() + nutritionValue);
        //incresae the health of the fish by the nutrition value of the food
        consumer.setHealth(consumer.getHealth() + nutritionValue);
        //SPEED IS NOW WORKING WITH ENERGY increase the speed of the fish by 0.1
        //consumer.setSpeed(consumer.getSpeed() + 0.1f);
        //here we don't have cooldown for the fish
        consumer.searchFoodCooldown = 0f; //0 seconds cooldown
    }
}
