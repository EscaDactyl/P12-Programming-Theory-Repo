using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiePosToPlayer : MonoBehaviour
{
    // The simplest script
    void Update()
    {
        transform.position = PlayerController.instance.playerPos;
    }
}
