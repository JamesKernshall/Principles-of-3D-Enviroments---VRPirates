using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squasher : MonoBehaviour
{
    [Header("REQUIRED")]
    [SerializeField] TriggerEvents3D pedestialTrigger;
    [SerializeField] Lever lever;
    [SerializeField] Transform hydralicPress;
    [SerializeField] Animator door;
    [SerializeField] Transform oilSpawn;
    [SerializeField] GameObject oilFullCan;
    [SerializeField] AudioSource audio3DSource;
    

    [Space]

    [Header("HydralicPress Parameters")]
    [SerializeField] float minScale = 10;
    [SerializeField] float maxScale = 20;
    [SerializeField] AudioClip SquishAudio;

    float scaleRange;

    GameObject objectInserted = null;
    
    SquasherState state = SquasherState.idle;

    enum SquasherState 
    {
        idle,
        itemInsert,
        WaitingForPickup
    }

    // Start is called before the first frame update
    void Start()
    {
        scaleRange = maxScale - minScale;
        pedestialTrigger.OnTriggerEnter3D += ObjectInsert;
        lever.OnThresholdPulled += OnLeverPulled;
    }

    void ObjectInsert(Collider other)
    {
        if (state == SquasherState.idle)
        {
            Rigidbody otherRigid = other.GetComponentInParent<Rigidbody>();

            if (otherRigid == null)
            {
                return;
            }
            else if (otherRigid.GetComponent<FruitIdentifier>() == null)
            {
                return;
            }

            otherRigid.transform.position = pedestialTrigger.transform.position;
            otherRigid.isKinematic = true;
            otherRigid.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable>().enabled = false;
            objectInserted = otherRigid.gameObject;
            state = SquasherState.itemInsert;
        }

    }

    private void Update()
    {
        // Lever value is normalised between -1, 1
        float leverValue = lever.CalculateHingeAngle();

        hydralicPress.localScale = new Vector3(hydralicPress.localScale.x, hydralicPress.localScale.y,
                                                ClampToHydralicScaleRange(leverValue));
    }

    private float ClampToHydralicScaleRange(float value)
    {
         return minScale + (scaleRange * ((value + 1) / 2));
    }

    private void OnLeverPulled() 
    {
        if (state == SquasherState.itemInsert) 
        {
            audio3DSource.PlayOneShot(SquishAudio);
            Destroy(objectInserted); // Destroy original and replace with oil
            objectInserted = GameObject.Instantiate(oilFullCan, oilSpawn.position, oilSpawn.rotation, this.transform.parent);
            door.SetBool("Open", true);
            state = SquasherState.WaitingForPickup;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Rigidbody>().gameObject == objectInserted) 
        {
            state = SquasherState.idle;
            door.SetBool("Open", false);
        }
    }
}
