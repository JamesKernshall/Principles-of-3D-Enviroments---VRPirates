using System;
using UnityEngine;

public class TriggerEvents3D : MonoBehaviour
{
    public Action<Collider> OnTriggerEnter3D;
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnter3D?.Invoke(other);
    }
}
