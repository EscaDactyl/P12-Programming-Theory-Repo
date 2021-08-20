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
        if(collision.gameObject.CompareTag("Player") && !GameManager.instance.gameOver)
        {
            GameManager.instance.gameOver = true;
            StartCoroutine(collision.gameObject.GetComponent<PlayerController>().KillPlayer());
        }
    }

}
