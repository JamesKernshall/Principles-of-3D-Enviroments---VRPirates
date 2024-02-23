using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Ship : MonoBehaviour
{

    [Header("REQUIRED")]
    public Transform Environment;
    public SteeringWheel Helm;
    public Lever Throttle;
    public Lever VerticalHandBrake;

    public Engine engineVFX;

    [Space]

    [SerializeField] private float baseSpeed = 12;
    [SerializeField] private float elevationSpeed = 5;
    private Rigidbody rb;

    private float speed = 0;
    private float verticalSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb =  Environment.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        speed = baseSpeed * Throttle.CalculateHingeAngle();
        verticalSpeed = elevationSpeed * VerticalHandBrake.CalculateHingeAngle();

        rb.AddForce(transform.forward * (speed * -1), ForceMode.Acceleration);
        rb.AddForce(transform.up * (verticalSpeed), ForceMode.Acceleration);

        transform.Rotate(0, Helm.TotalSteeringAngle, 0);


        engineVFX.UpdateVFX(speed, verticalSpeed);
    }
}
