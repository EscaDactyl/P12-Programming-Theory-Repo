using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float lookAheadDistance;

    // This is a camera function so LateUpdate
    void LateUpdate()
    {
        // Only if it's not game over

        if (!GameManager.instance.gameOver)
        {
            transform.position = PlayerController.instance.playerPos + cameraOffset;
            transform.LookAt(PlayerController.instance.playerPos + Vector3.forward * lookAheadDistance);
        }
    }
}
