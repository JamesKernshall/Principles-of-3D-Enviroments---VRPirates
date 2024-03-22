using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Asteroids/FruitObject")]
public class FruitDecider : ScriptableObject
{
    public AsteroidSpawner.FruitAsteroidType fruitID;
    public GameObject Asteroid;
    public GameObject Grippable;
}
