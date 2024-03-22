using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoover : MonoBehaviour
{
    [Header("Sucking")]
    [SerializeField] private AudioClip grindingSFX;
    [SerializeField] float maxScale;
    [SerializeField] private GameObject grabbablePrefab;
    [SerializeField] private AudioSource grindingSFXSource;


    [Header("Firing")]
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private AudioClip fireSFX;
    [SerializeField] private float firePower;
    [SerializeField] private float fireScale;
    [SerializeField] private Transform firePosition;
    [SerializeField] private int maxObjectsLoaded = 100;


    [HideInInspector] public Transform environment;
    [HideInInspector] public Transform[] spawnLocations;
    [HideInInspector] public PhysicsButton physicsButton;
    [HideInInspector] public TriggerEvents3D fireInserter;


    private List<GameObject> loadedObjects = new List<GameObject>();
    
    /// <summary>
    /// Called by the ship after setting up all variables
    /// </summary>
    void LateStart() 
    {
        physicsButton.OnButtonPresssed += AttemptFire;
        fireInserter.OnTriggerEnter3D += OnInsertMag;
    }

    private void OnInsertMag(Collider other) 
    {
        if (loadedObjects.Count <= maxObjectsLoaded) 
        {
            GameObject loadedObject = other.GetComponentInParent<Rigidbody>().gameObject;
            loadedObject.SetActive(false);
            loadedObjects.Add(loadedObject);
            

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DestructibleAsteroid hitAsteroid = other.GetComponentInParent<DestructibleAsteroid>();
        if (hitAsteroid != null)
        {
            // Hit a hoverable object
            OnHitObject(hitAsteroid);
        }
    }

    void OnHitObject(DestructibleAsteroid other) 
    {
        float scale = other.transform.localScale.x;

        if (scale < maxScale) 
        {
            int amountOfObjects = (int)(scale / fireScale); // - Firescale to keep a semi-consistent sizing

            if (amountOfObjects <= 0)
                amountOfObjects = 1;

            grindingSFXSource.Play();

            GameObject prefab = grabbablePrefab;

            FruitDecider fruitInfo = FruitManager.instance.GetInfoByID(other.fruit);

            if (fruitInfo != null) 
            {
                prefab = fruitInfo.Grippable;
            }
            

            // Put this into a corroutine with a slight delay then extend the grinding source
            for(int i = 0; i < amountOfObjects; i++) 
            {
                Transform spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Length)];
                GameObject newObject = GameObject.Instantiate(prefab, spawnLocation.position, spawnLocation.rotation, spawnLocation);
                
                MeshFilter oldFilter = other.GetComponentInChildren<MeshFilter>();
                MeshFilter newFilter = newObject.GetComponentInChildren<MeshFilter>();

                SwapRenderers(oldFilter, newFilter);

                newObject.GetComponentInChildren<Rigidbody>().mass /= amountOfObjects;
                newObject.transform.localScale = Vector3.one;
            }


            GameObject.Destroy(other.gameObject);
        }
    }

    static void SwapRenderers(MeshFilter oldFilter, MeshFilter newFilter) 
    {
        newFilter.sharedMesh = oldFilter.sharedMesh; // Grab previous object's mesh / materials
        newFilter.GetComponent<MeshRenderer>().sharedMaterials = oldFilter.GetComponent<MeshRenderer>().sharedMaterials;

    }

    void AttemptFire() 
    {
        if (loadedObjects.Count > 0) 
        {
            FireObject(loadedObjects[0]);
            loadedObjects.RemoveAt(0);
        }
    }

    public void FireObject(GameObject fireObject) 
    {
        AudioSource.PlayClipAtPoint(fireSFX, transform.position);

        GameObject projectile = GameObject.Instantiate(firePrefab, firePosition.position, firePosition.rotation, environment);
        projectile.transform.localScale *= fireScale;

        //Update Projectile Mesh
        projectile.GetComponentInChildren<MeshFilter>().sharedMesh = fireObject.GetComponentInChildren<MeshFilter>().sharedMesh;
        projectile.GetComponentInChildren<MeshRenderer>().sharedMaterial = fireObject.GetComponentInChildren<MeshRenderer>().sharedMaterial;

        Rigidbody newRB = projectile.GetComponent<Rigidbody>();
        newRB.AddForce((transform.forward * -1) * firePower, ForceMode.VelocityChange);

        Destroy(fireObject);
    }

}
