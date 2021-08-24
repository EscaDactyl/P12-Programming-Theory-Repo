using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1001)] // Right after PlayerController
public class BallzBomb : Ballz
{

    float countdownTime;
    
    [SerializeField] GameObject explosionPfxObj;
    [SerializeField] float bombStrength;
    [SerializeField] float bombRadius;

    // Start is called before the first frame update
    void Start()
    {
        // to get time, take the distance and divide by a little bit over the max possible player velocity so that if the player is always sprinting, the bomb goes off
        countdownTime = (transform.position.z - PlayerController.instance.playerPos.z) / (PlayerController.instance.maxRunSpeed * 1.25f);
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

        ExplodeBomb();
    }

    void ExplodeBomb()
    {
        GameObject thisExplosion = Instantiate(explosionPfxObj, transform.position, explosionPfxObj.transform.rotation);
        thisExplosion.GetComponent<ParticleSystem>().Play();

        // Find all non-track rigidbodies within this radius and apply an explosion force.
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, bombRadius);
        foreach (Collider collider in detectedColliders)
        {
            if (!collider.CompareTag("Track") && collider.attachedRigidbody != null)
            {
                collider.attachedRigidbody.AddExplosionForce(bombStrength, transform.position, bombRadius);
            }
        }

        Destroy(gameObject);
    }

    protected override void OnTriggerEnter(Collider other) // Bombs explode instead of being flung off
    {
        if (other.gameObject.CompareTag("BlastBurstSpell"))
        {
            ExplodeBomb();
        }
    }
}
