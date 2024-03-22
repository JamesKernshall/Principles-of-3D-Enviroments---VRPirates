using UnityEngine;
using System;

public class PhysicsButton : MonoBehaviour
{
    public Action OnButtonPresssed;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I'm pressed!");
        //Temp function before I get the button actually physics based
        OnButtonPresssed?.Invoke();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("I'm pressed!");
        //Temp function before I get the button actually physics based
        OnButtonPresssed?.Invoke();    
    }
}
