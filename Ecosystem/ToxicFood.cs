using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicFood : Food 
{
    public float fallSpeed = 0.5f;

    void Start()
    {
        //we can use the singleton pattern of the FlockManager to get the nutrition value of the food
        //if we want to change the nutrition value during the simulation, we can do it on the consume method
        nutritionValue = FlockManager.FM.nutritionValueToxic;
    }

    //fall each frame
    void Update()
    {
        Fall();
    }

    //falling method
    void Fall()
    {
        //if the food is at the y axis down 1,2, stop moving
        if (transform.position.y < -1.2f)
        {
            //stop moving
        }
        else
        {
            //we move the food down slowly
            transform.Translate(Vector3.down * Time.deltaTime * fallSpeed);
        }
    }

    public override void Consume(Simulation consumer) 
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
