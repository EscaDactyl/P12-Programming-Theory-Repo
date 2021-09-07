using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallzEye : Ballz
{
    public float homingStrength = 10.0f;
    private float sightDistance = 100f;

    void Update()
    {
        if (GameManager.instance.gameActive && DistanceToPlayer() < sightDistance)
        {
            PushEyeTowardsPlayer();
        }
    }
    private void PushEyeTowardsPlayer()
    {
        Rigidbody eyeRb = GetComponent<Rigidbody>();

        Vector3 normalizedDirection = (PlayerController.instance.playerPos - transform.position).normalized;

        eyeRb.AddForce(normalizedDirection * eyeRb.mass * homingStrength * Time.deltaTime, ForceMode.Force);
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(PlayerController.instance.playerPos, transform.position);
    }
}
