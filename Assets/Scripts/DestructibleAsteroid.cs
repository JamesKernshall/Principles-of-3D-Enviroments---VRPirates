using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleAsteroid : MonoBehaviour
{
    public AsteroidSpawner.FruitAsteroidType fruit;
    [SerializeField] float explosiveForce;
    [SerializeField] float minScale;
    [SerializeField] float divideAmount;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            Destroy(other.GetComponentInParent<Rigidbody>().gameObject);

            Vector3 explosionPos = transform.position;

            float dividedScale = this.transform.localScale.x / divideAmount;
            Destroy(this.gameObject);

            if (dividedScale > minScale)
            {
                for (int i = 0; i < Mathf.FloorToInt(divideAmount); i++)
                {
                    GameObject newObject = GameObject.Instantiate(this.gameObject, this.transform.position, this.transform.rotation, this.transform.parent);
                    Vector3 explosionTarget = gameObject.transform.position + new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20)) * (explosiveForce);
                    newObject.GetComponent<DestructibleAsteroid>().ExplodeToTarget(explosionTarget);
                    newObject.transform.localScale /= divideAmount;
                }
            }
        }
    }


    public void ExplodeToTarget(Vector3 posTarget) 
    {
        this.divideAmount /= 1.5f;
        StartCoroutine(Explode(posTarget));    
    }



    IEnumerator Explode(Vector3 posTarget) 
    {
        float time = 0;
        while (Vector3.Distance(transform.position, posTarget) > 0.3)
        {
            time += Time.deltaTime / (explosiveForce);
            transform.position = Vector3.Lerp(transform.position, posTarget, time);
            
             yield return new WaitForFixedUpdate();
        }
    }
}
