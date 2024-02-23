using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class Lever : MonoBehaviour
{
    public static Action OnThresholdPulled;

    [Header("STATS")]
    [Range(-1,1)] [SerializeField] float minThreshold;
    [Range(-1,1)] [SerializeField] float maxThreshold;

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
            leverPulled = false;
            OnThresholdPulled?.Invoke();
        }
        else if (currentAngle > maxThreshold)
        {
            leverPulled = true;
            OnThresholdPulled?.Invoke();
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {

            float currentAngle = CalculateHingeAngle();

            CheckThresholds(currentAngle);

            text.text = $"Current Angle = {CalculateHingeAngle()}";
        }
    }


        public float CalculateHingeAngle() 
    {
        float min = hinge.limits.min;
        float max = hinge.limits.max;

        float normalized = ((2 * (hinge.angle - min)) / (max - min)) - 1;

        return normalized;
    }
}
