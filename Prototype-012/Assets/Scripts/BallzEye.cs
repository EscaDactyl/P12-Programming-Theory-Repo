using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallzEye : Ballz
{
    public float homingStrength = 10.0f;

    void Update()
    {
        PushEyeTowardsPlayer();
    }
    private void PushEyeTowardsPlayer()
    {
        Rigidbody eyeRb = GetComponent<Rigidbody>();

        Vector3 normalizedDirection = (PlayerController.instance.playerPos - transform.position).normalized;

        eyeRb.AddForce(normalizedDirection * eyeRb.mass * homingStrength * Time.deltaTime, ForceMode.Force);
    }
}
