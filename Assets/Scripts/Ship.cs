using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Ship : MonoBehaviour
{

    [Header("REQUIRED")]
    public Transform Environment;
    public Transform ExtraObjectsThatNeedRotating;
    public SteeringWheel Helm;
    public Lever Throttle;
    public Lever VerticalHandBrake;

    [Header("- Hoover -")]
    public Hoover FrontHoover;
    public PhysicsButton FireButton;
    public Transform[] ChestSpawnLocations;
    public TriggerEvents3D FireInserter;

    public Engine engineVFX;

    [Space]

    [SerializeField] private float baseSpeed = 12;
    [SerializeField] private float elevationSpeed = 5;
    private Rigidbody rb;

    private float speed = 0;
    private float verticalSpeed = 0;

    private void Awake()
    {
        FrontHoover.physicsButton = FireButton;
        FrontHoover.fireInserter = FireInserter;
        FrontHoover.environment = Environment;
        FrontHoover.SendMessage("LateStart");

        engineVFX.topSpeed = baseSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        FrontHoover.spawnLocations = ChestSpawnLocations;

        rb =  Environment.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        speed = baseSpeed * Throttle.CalculateHingeAngle();
        verticalSpeed = elevationSpeed * VerticalHandBrake.CalculateHingeAngle();

        rb.AddForce(transform.forward * (speed * -1), ForceMode.Acceleration);
        rb.AddForce(transform.up * (verticalSpeed), ForceMode.Acceleration);

        transform.Rotate(0, Helm.TotalSteeringAngle, 0);
        ExtraObjectsThatNeedRotating.rotation = transform.rotation;


        engineVFX.UpdateVFX(speed, verticalSpeed);
    }
}
