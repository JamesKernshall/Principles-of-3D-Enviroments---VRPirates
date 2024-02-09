using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class SteeringWheel : XRBaseInteractable
{
    [SerializeField] private Transform wheelTransform;

    public UnityEvent<float> OnWheelRotated;

    public float steeringSens = 1;
    public int maxRegisteredSpins = 2;

    private float currentGrabAngle = 0.0f;

    [Space]
    [Header("Debug")]
    public Vector3 angularVelocity;
    [SerializeField] private bool debugRotation = false;
    [SerializeField] private Transform debugObject;


    [Space]
    [SerializeField] private bool isVelocityBased = false;
    [SerializeField] private float velocityDrag = 1;
    [SerializeField] private float maxAngularZVelocity;
    [SerializeField] private float accerlationModifier = 3;
    [SerializeField] private float bounceBackModifier = 0.33f;
    private float rotationTotal = 0;
    private Quaternion lastRotation;
    private float lastSteeringAngle = 0;
    private bool isGrabbed = false;

    /// <summary>
    /// Grabs the total steering angle including multiple rotations;
    /// </summary>
    public float TotalSteeringAngle
    {
        get
        {
            return rotationTotal;
        }
    }


    /// <summary>
    /// Get's the wheel's current steering angle from a value -1 to 1
    /// </summary>
    public float SteeringAngle 
    {
        get 
        {
            // I hate this, why doesn't it just work with quaterions??
            return wheelTransform.rotation.eulerAngles.z / 180;
        }
    }

    private void Start()
    {
        wheelTransform.rotation = Quaternion.identity;
    }

    private void VelocityUpdate() 
    {
        if (angularVelocity.z > maxAngularZVelocity) // Stop silly speeds
        {
            angularVelocity.z = maxAngularZVelocity;
        }
        else if (angularVelocity.z < -maxAngularZVelocity) // Stop silly speeds
        {
            angularVelocity.z = -maxAngularZVelocity;
        }


        wheelTransform.Rotate(transform.forward * angularVelocity.z, Space.World);
        ClampRotation();


        angularVelocity *= (1 / velocityDrag);
    }

    private void FixedUpdate()
    {
        if (isVelocityBased && !isGrabbed) // Only ran when not grabbed
        {
            if (angularVelocity.sqrMagnitude > 0.1f)
            {
                VelocityUpdate();
            }
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        currentGrabAngle = FindWheelAngle();
        lastRotation = wheelTransform.rotation;
        isGrabbed = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        currentGrabAngle = FindWheelAngle();
        angularVelocity *= accerlationModifier;
        isGrabbed = false;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
                RotateWheel();
        }
    }

    private void RotateWheel()
    {
        // Convert that direction to an angle, then rotation
        float totalAngle = FindWheelAngle();

        // Apply difference in angle to wheel
        float angleDifference = currentGrabAngle - totalAngle;

        if (isVelocityBased) // To mimic the steering of a pirate ship
        {
            angularVelocity = wheelTransform.rotation.eulerAngles - lastRotation.eulerAngles; // Could be moved to on exit
            lastRotation = wheelTransform.rotation;

            wheelTransform.Rotate(transform.forward, -angleDifference, Space.World);
            ClampRotation(); // Clamps value to min / Max
        }
        else
            wheelTransform.Rotate(transform.forward, -angleDifference, Space.World);

        // Store angle for next process
        currentGrabAngle = totalAngle;
        OnWheelRotated?.Invoke(angleDifference);
    }

    private float FindWheelAngle()
    {
        float totalAngle = 0;

        // Combine directions of current interactors
        foreach (IXRSelectInteractor interactor in interactorsSelecting)
        {
            Vector2 direction = FindLocalPoint(interactor.transform.position);
            totalAngle += ConvertToAngle(direction) * FindRotationSensitivity();
        }

        return totalAngle;
    }



    /// <summary>
    /// Due to async stuff to do with the interactables you can't clamp the motion. 
    /// This instead focuses on counting the number of rotations and clamping that
    /// </summary>
    private void ClampRotation() 
    {
        /*
        if (wheelTransform.rotation.z != Mathf.Clamp(wheelTransform.rotation.z, minValue, maxValue)) // Check if it's hit the edge
        {
            angularVelocity *= -bounceBackModifier;
            currentGrabAngle *= -bounceBackModifier;
        }

        wheelTransform.rotation.Set(wheelTransform.rotation.x, wheelTransform.rotation.y, Mathf.Clamp(wheelTransform.rotation.z, minValue, maxValue), wheelTransform.rotation.w);*/

        /* // TODO: Swap to this
        
        if (lastSteeringAngle < -0.5f && SteeringAngle > 0.5) 
        {
            //Positive
            rotationNumber++;
        }
        else if (lastSteeringAngle > 0.5 && SteeringAngle < -0.5) 
        {
            // Negative
            rotationNumber--;
        }
        */

        // rotationNumber = Mathf.Clamp(rotationNumber, -maxRegisteredSpins, maxRegisteredSpins);


        float rotationOffset = lastSteeringAngle - SteeringAngle;

        if (-0.5f < rotationOffset && rotationOffset < 0.5f)
        {
            rotationTotal += rotationOffset;
        }
        else 
        {
            Debug.Log("Swap over!");
        }

        lastSteeringAngle = SteeringAngle;

        if (debugRotation) 
        {
            debugObject.position = new Vector3(TotalSteeringAngle, 0);
        }

    }

    private Vector2 FindLocalPoint(Vector3 position)
    {
        // Convert the hand positions to local, so we can find the angle easier
        return transform.InverseTransformPoint(position).normalized;
    }

    private float ConvertToAngle(Vector2 direction)
    {
        // Use a consistent up direction to find the angle
        return Vector2.SignedAngle(Vector2.up, direction);
    }

    private float FindRotationSensitivity()
    {
        // Use a smaller rotation sensitivity with two hands
        return steeringSens / interactorsSelecting.Count;
    }
}