using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAnimations : MonoBehaviour
{
    [SerializeField] private GameObject[] animators;
    [SerializeField] private float timeDifference = 0.68f;


    private void Awake()
    {
        foreach (GameObject anim in animators) 
        {
            anim.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        foreach (GameObject anim in animators) 
        {
            anim.SetActive(true);
            yield return new WaitForSeconds(timeDifference);
        }
    } 
}
