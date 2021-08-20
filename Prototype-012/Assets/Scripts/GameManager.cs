using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    
    [SerializeField] GameObject testBall;

    public Camera currentCamera;

    public bool gameOver = false;
    void Awake()
    {
        instance = this;
        currentCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        InvokeRepeating("SpawnABall", 5f, 1f);
    }

    private void SpawnABall()
    {
        float xPos = Random.Range(-1, 2) * 1.616f;
        float zOffset = Random.Range(50f, 150f);
        Vector3 spawnPos = new Vector3(xPos, 12f, PlayerController.instance.playerPos.z + zOffset);

        GameObject thisBall = Instantiate(testBall, spawnPos, testBall.transform.rotation);

        Rigidbody thisRb = thisBall.GetComponent<Rigidbody>();

        thisRb.AddForce(Random.Range(0f, 5f) * thisRb.mass * Vector3.back, ForceMode.Impulse);
        thisRb.AddTorque(new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)) * thisRb.mass, ForceMode.Impulse);
    }

}
