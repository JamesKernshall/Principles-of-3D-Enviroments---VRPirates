using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{

    [SerializeField] ParticleSystem engineVFX;
    [SerializeField] AudioSource engineAudio;

    [HideInInspector] public float topSpeed = 80;
    [SerializeField] AnimationCurve pitchGradient;
    [SerializeField] AnimationCurve volumeGradient;

    float engineVFXStartScale = 0;

    private void Start()
    {
        engineVFXStartScale = engineVFX.transform.localScale.z;
    }
    public void UpdateVFX(float speed, float verticalSpeed)
    {
        speed = Mathf.Abs(speed);
        verticalSpeed = Mathf.Abs(verticalSpeed);

        float higherSpeed;
        if (speed > verticalSpeed)
             higherSpeed = speed;
        else higherSpeed = verticalSpeed;

        engineAudio.pitch = pitchGradient.Evaluate(higherSpeed / topSpeed);
        engineAudio.volume = volumeGradient.Evaluate(higherSpeed / topSpeed);

        engineVFX.transform.localScale = new Vector3(engineVFX.transform.localScale.x,
                                                     engineVFX.transform.localScale.y,
                                                     engineVFXStartScale + speed / topSpeed);

    }
}
