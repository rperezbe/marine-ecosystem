using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthyFood : Food
{
    public override void Consume(Flock consumer) 
    {
        //increase the energy of the fish by the nutrition value of the food
        //consumer.setEnergy(consumer.getEnergy() + nutritionValue);

        //increase the energy of the fish to 100
        consumer.setEnergy(100);
        //incresae the health of the fish by the nutrition value of the food
        consumer.setHealth(consumer.getHealth() + nutritionValue);
        //increase the speed of the fish by 0.1
        consumer.setSpeed(consumer.getSpeed() + 0.1f);
        //here we don't have cooldown for the fish
        consumer.searchFoodCooldown = 0f; //0 seconds cooldown
    }
}
