using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int nutritionValue; //the nutrition value of the food

    public virtual void Consume(Simulation consumer)
    {
        //empty
    }
}
