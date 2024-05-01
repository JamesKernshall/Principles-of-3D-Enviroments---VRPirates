using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class Lever : MonoBehaviour
{
    public Action OnThresholdPulled;

    [Header("STATS")]
    [Range(-1,1)] [SerializeField] float minThreshold;
    [Range(-1,1)] [SerializeField] float maxThreshold;
    [Range(0,1)] [SerializeField] float deadZone;


    [Space]

    [Header("DEBUG NOT REQUIRED")]
    [SerializeField] bool debug;
    [SerializeField] TextMesh text;


    HingeJoint hinge;
    Rigidbody shipRB;
    Rigidbody rb;
    bool leverPulled = false;

    Vector3 offsetFromParent;

    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();
        if (!debug && text != null) 
        {
            text.gameObject.SetActive(false);
        }
    }

    void CheckThresholds(float currentAngle) 
    {
        if (currentAngle < minThreshold)
        {
            if (leverPulled) // Stops duplicate invokes
            {
                OnThresholdPulled?.Invoke();
            }
            leverPulled = false;
        }
        else if (currentAngle > maxThreshold)
        {
            if (!leverPulled) // Stops duplicate invokes
            {
                OnThresholdPulled?.Invoke();
            }
            leverPulled = true;
        }


    }

    // Update is called once per frame
    void Update()
    {
        float currentAngle = CalculateHingeAngle();

        CheckThresholds(currentAngle);

        if (debug && text != null)
        {
            text.text = $"Current Angle = {CalculateHingeAngle()}";
        }
    }


        public float CalculateHingeAngle() 
    {
        float min = hinge.limits.min;
        float max = hinge.limits.max;

        float normalized = ((2 * (hinge.angle - min)) / (max - min)) - 1;


        if (normalized > -deadZone && deadZone > normalized) 
        {
            normalized = 0;
        }

        return normalized;
    }
}
