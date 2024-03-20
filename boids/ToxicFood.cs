using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicFood : Food 
{
    public override void Consume(Flock consumer) 
    {
        //toxic food doesn't decrease the health but it decreases the energy and the speed of the fish
        //decrease the energy of the fish by the nutrition value of the food
        consumer.setEnergy(consumer.getEnergy() - nutritionValue);
        //SPEED IS NOW WORKING WITH ENERGY decrease the speed of the fish by 0.1
        //consumer.setSpeed(consumer.getSpeed() - 0.1f);
        //adjust the cooldown of the fish
        consumer.searchFoodCooldown = 5.0f; //5 seconds cooldown
    }
}
