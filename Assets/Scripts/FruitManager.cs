using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    public static FruitManager instance;

    public Action<float> oilDrank;

    public static void OnOilDrink(float amount) 
    {
        instance.oilDrank?.Invoke(amount);
    }

    public List<FruitDecider> fruits = new List<FruitDecider>();
    private void Awake()
    {
        instance = this;
    }


    public FruitDecider GetInfoByID(AsteroidSpawner.FruitAsteroidType fruitID) 
    {
        foreach (FruitDecider fruit in fruits)
        {
            if (fruit.fruitID == fruitID)
            {
                return fruit;
            }
        }

        return null;
    }

}
