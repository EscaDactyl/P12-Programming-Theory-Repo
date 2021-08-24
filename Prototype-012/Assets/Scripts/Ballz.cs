using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballz : MonoBehaviour
{

    private void Update()
    {
        float lowerBound = -100f;

        if(transform.position.y < lowerBound)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Hits player, not game over, and not invincible from dashing
        if(collision.gameObject.CompareTag("Player") && !GameManager.instance.gameOver && PlayerController.instance.dashSpeed == 0)
        {
            GameManager.instance.gameOver = true;
            StartCoroutine(PlayerController.instance.KillPlayer());
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Rigidbody thisRb = GetComponent<Rigidbody>();
        float explosionStrength = 100000f;
        float blastBurstRadius = 30f;

        if(other.gameObject.CompareTag("BlastBurstSpell"))
        {
            thisRb.AddExplosionForce(explosionStrength, PlayerController.instance.playerPos, blastBurstRadius);
        }
    }

}
