using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallzBomb : Ballz
{

    public float countdownTime;
    
    [SerializeField] GameObject explosionPfxObj;
    [SerializeField] float bombStrength;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BeginCountdown());
    }


    IEnumerator BeginCountdown()
    {
        float timeElapsed = 0.0f;

        while(timeElapsed < countdownTime)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
        }


        GameObject thisExplosion = Instantiate(explosionPfxObj, transform.position, explosionPfxObj.transform.rotation);
        thisExplosion.GetComponent<ParticleSystem>().Play();

        // Find all rigidbodies within this radius and apply an explosion force.
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, 1.2f);
        foreach(Collider collider in detectedColliders)
        {
            if(!collider.CompareTag("Track"))
            {
                collider.attachedRigidbody.AddExplosionForce(bombStrength * collider.attachedRigidbody.mass, transform.position, 1.2f);
            }
        }

        Destroy(gameObject);

    }
}
