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
        // Only if it's game active

        if (GameManager.instance.gameActive)
        {
            transform.position = PlayerController.instance.playerPos + cameraOffset;
            transform.LookAt(PlayerController.instance.playerPos + Vector3.forward * lookAheadDistance);
        }
    }
}
