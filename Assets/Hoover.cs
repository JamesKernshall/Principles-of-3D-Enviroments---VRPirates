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
    [SerializeField] private AudioClip fireSFX;
    [SerializeField] private float firePower;
    [SerializeField] private float fireScale;
    [SerializeField] private Transform firePosition;
    [SerializeField] private int maxObjectsLoaded = 100;



    [HideInInspector] public Transform[] spawnLocations;
    [HideInInspector] public PhysicsButton physicsButton;
    [HideInInspector] public TriggerEvents3D fireInserter;


    private List<GameObject> loadedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
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
        if (other.CompareTag("Hooverable"))
        {
            // Hit a hoverable object
            OnHitObject(other);
        }
    }

    void OnHitObject(Collider other) 
    {
        float scale = other.transform.localScale.magnitude;
        Debug.Log(scale);
        if (scale > maxScale) 
        {
            int amountOfObjects = (int)(scale / Vector3.one.magnitude);

            grindingSFXSource.Play();

            // Put this into a corroutine with a slight delay then extend the grinding source
            for(int i = 0; i < amountOfObjects; i++) 
            {
                Transform spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Length)];
                GameObject newObject = GameObject.Instantiate(grabbablePrefab, spawnLocation.position, spawnLocation.rotation, spawnLocation);
                
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

        Rigidbody newRB = fireObject.GetComponent<Rigidbody>();
        fireObject.transform.position = firePosition.position;
        fireObject.transform.rotation = firePosition.rotation;
        fireObject.transform.localScale *= fireScale;
        fireObject.SetActive(true);

        newRB.AddForce(Vector3.forward * firePower, ForceMode.VelocityChange);
    }

}
