using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{

    /// <summary>
    /// NOTE TO SELF:
    /// CREATE A TRIGGER WHEN THE PLAYER HITS A CERTAIN DISTANCE AND SPAWN MORE ASTEROIDS
    /// ALSO CHECK IF A  PARTICULAR ASTEROID IS TOO FAR AWAY AND DESTROY IT. WORK OFF WORLD 0,0,0
    /// </summary>
    /// 

    public enum FruitAsteroidType
    {
        UNKNOWN,
        apple,
        banana,
        pineapple,
        strawberry
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
