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

    [Header("REQUIRED")]
    [SerializeField] private Transform Environment;
    [SerializeField] private Transform Ship;

    [Space]

    [Header("Spawning Parameters")]
    public float maxDistance = 300;
    [SerializeField] int minAsteroids = 10;[SerializeField] int maxAsteroids = 30;
    [SerializeField] int minScale = 5;[SerializeField] int maxScale = 80;



    private List<DestructibleAsteroid> asteroid_list = new List<DestructibleAsteroid>();


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
        InitAsteroids();
    }

    void InitAsteroids()
    {
        for (int i = 0; i < Random.Range(minAsteroids, maxAsteroids); i++)
        {
            SpawnAsteroid();
        }
    }


    private void SpawnAsteroid()
    {
        // Pick random FruitType
        FruitAsteroidType randomFruit = (FruitAsteroidType)Random.Range(1,5);

        FruitDecider fruitData = FruitManager.instance.GetInfoByID(randomFruit);

        Vector3 randomSpawnPosition = new Vector3(Random.Range(-maxDistance, maxDistance) * 0.9f, Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance) * 0.9f);
        //randomSpawnPosition.Scale(Ship.forward);
        Quaternion randomRotation = Random.rotation;
        float randomScale = Random.Range(minScale, maxScale);

        DestructibleAsteroid newAsteroid = GameObject.Instantiate(fruitData.Asteroid, randomSpawnPosition, randomRotation, Environment).GetComponent<DestructibleAsteroid>();
        if (newAsteroid == null)
        {
            Debug.LogError($"SPAWNED ASTEROID - {newAsteroid.name} DOESN'T HAVE DESTRUCTIBLE ASTEROID DATA");
        }
        else
        {
            RegisterAsteroid(newAsteroid);
            newAsteroid.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            newAsteroid.fruit = fruitData.fruitID;

        }


    }

    public void RegisterAsteroid(DestructibleAsteroid asteroid)
    {
        asteroid_list.Add(asteroid);
    }
    public void UnRegisterAsteroid(DestructibleAsteroid asteroid)
    {
        asteroid_list.Remove(asteroid);
    }

    private void FixedUpdate()
    {
        CheckAsteroidDistances();
    }

    private void CheckAsteroidDistances()
    {
        List<DestructibleAsteroid> hitList = new List<DestructibleAsteroid>();
        foreach (DestructibleAsteroid asteroid in asteroid_list)
        {
            if (asteroid.transform.position.magnitude > (maxDistance * Vector3.one.magnitude))
            {
                //Mark for deletion later
                hitList.Add(asteroid);
            }
        }

        foreach (DestructibleAsteroid assassination in hitList) 
        {
            asteroid_list.Remove(assassination);
            Destroy(assassination.gameObject);
            SpawnAsteroid();
        }

        
        /*
        else if (asteroid_list.Count < minAsteroids)
        {
            //Spawn a handful of asteroids to keep around the middle amount of asteroids
            for (int i = 0; i < Random.Range(minAsteroids, minAsteroids + ((maxAsteroids - minAsteroids) / 3)); i++)
            {
                SpawnAsteroid();
            }
        }*/
    }
}
