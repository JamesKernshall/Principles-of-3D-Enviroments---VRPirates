using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateHeight : MonoBehaviour
{
    [SerializeField] private Transform cameraOffset;
    [SerializeField] private Transform targetPos;
    [SerializeField] private Transform headsetPos;

    // Start is called before the first frame update
    void Start()
    {
        Calibrate();
    }
    
    [ContextMenu("Calibrate Height - Based On Target")]
    public void Calibrate() 
    {
        float yOffset = targetPos.position.y - cameraOffset.position.y;
        cameraOffset.position = new Vector3(cameraOffset.position.x, cameraOffset.position.y + yOffset, cameraOffset.position.z);
    }

}
