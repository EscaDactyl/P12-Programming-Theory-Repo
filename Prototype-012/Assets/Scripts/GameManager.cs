using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    struct BallStruct
    {
        public GameObject BallObj {get;}
        public float Odds { get; }
        public float MinRandomRange { get; set; }
        public float MaxRandomRange { get; set; }

        public BallStruct(GameObject ballObj, float odds)
        {
            // will need to insert errorchecking
            BallObj = ballObj;
            Odds = odds;
            MinRandomRange = 0;
            MaxRandomRange = odds;
        }
    }
    

    public static GameManager instance;
    
    BallStruct[] ballz;
    [SerializeField] GameObject[] editBallz;
    [SerializeField] float[] editOddz;

    public Camera currentCamera;

    public bool gameOver = false;
    void Awake()
    {
        instance = this;
        currentCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        InitializeBallz();
        InvokeRepeating("SpawnABall", 5f, 1f);
    }

    private void InitializeBallz()
    {
        int arrayLength = editBallz.Length;
        ballz = new BallStruct[arrayLength];
        float totalOddz = 0.0f;
        float cumulOddz = 0.0f;

        for (int n = 0; n < arrayLength; n++)
        {
            ballz[n] = new BallStruct(editBallz[n], editOddz[n]);
            totalOddz += editOddz[n];
        }

        for (int n = 0; n < arrayLength; n++)
        {
            ballz[n].MinRandomRange = cumulOddz;
            ballz[n].MaxRandomRange = cumulOddz + ballz[n].Odds / totalOddz;
            cumulOddz = ballz[n].MaxRandomRange;
        }
    }

    private int PickABallIndex()
    {
        float rng = Random.value;

        for(int n = 0; n < ballz.Length; n++)
        {
            if(rng >= ballz[n].MinRandomRange && rng < ballz[n].MaxRandomRange)
            {
                return n;
            }
        }

        // so unlikely but i'll feel better
        return ballz.Length;
    }
    
    private void SpawnABall()
    {
        float xPos = Random.Range(-1, 2) * 1.616f;
        float zOffset = Random.Range(50f, 150f);
        GameObject ballToSpawn = ballz[PickABallIndex()].BallObj;
        Vector3 spawnPos = new Vector3(xPos, 12f, PlayerController.instance.playerPos.z + zOffset);

        GameObject thisBall = Instantiate(ballToSpawn, spawnPos, ballToSpawn.transform.rotation);

        Rigidbody thisRb = thisBall.GetComponent<Rigidbody>();

        thisRb.AddForce(Random.Range(0f, 5f) * thisRb.mass * Vector3.back, ForceMode.Impulse);
        thisRb.AddTorque(new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)) * thisRb.mass, ForceMode.Impulse);
    }
    
}
