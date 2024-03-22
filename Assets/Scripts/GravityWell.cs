using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        TriggerActive(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerActive(other, false);
    }

    private void TriggerActive(Collider other, bool Enter) 
    {
        Rigidbody rigidCollider = other.GetComponentInParent<Rigidbody>();

        if (rigidCollider != null) 
        {
            rigidCollider.useGravity = Enter;   
        }

    }
}
