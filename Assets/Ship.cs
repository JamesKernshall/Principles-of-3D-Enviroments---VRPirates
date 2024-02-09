using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Ship : MonoBehaviour
{

    [Header("REQUIRED")]
    public SteeringWheel Helm;

    [Space]

    [SerializeField] private float speed = 4;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed, ForceMode.Acceleration);


        transform.Rotate(0, Helm.TotalSteeringAngle, 0);
    }
}
