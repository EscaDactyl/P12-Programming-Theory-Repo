using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // It's called BallStruct but it spawns powerups too
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
    public Camera currentCamera;
    public bool gameOver = false;

    public void SpawnAThing(float zPos)
    {
        int slots = TrackManager.instance.TrackSections[0].GetComponent<TrackPieceManager>().TrackChildCount;
        int slotAdj = 0;
        if (slots % 2 == 1)
        {
            slotAdj = 1;
        }
        float xPos = Random.Range(-(slots - slotAdj) / 2, (slots - slotAdj) / 2 + 1) * 1.616f;
        float yOffset = 0.5f;
        float startingY = 5.0f;
        Vector3 raycastTopPos = new Vector3(xPos, startingY, zPos);

        RaycastHit colliderDetected;
        bool didCastHit = Physics.Raycast(raycastTopPos, Vector3.down, out colliderDetected, startingY * 2, LayerMask.GetMask("Track")); // This should get the raycast hit info for the position desired
        float yPos = startingY - colliderDetected.distance + yOffset;

        GameObject ballToSpawn = ballz[PickABallIndex()].BallObj;
        Vector3 spawnPos = new Vector3(xPos, yPos, zPos);

        /* Testing code
        if (didCastHit)
        {
            Debug.Log("didCastHit: " + didCastHit + " Detected Pos: " + (startingY - colliderDetected.distance) + " collider transform: " + colliderDetected.transform.position);
        }
        else
        {
            Debug.LogError("didCastHit: " + didCastHit);
        }
        */

        GameObject thisBall = Instantiate(ballToSpawn, spawnPos, ballToSpawn.transform.rotation);

        if (thisBall.CompareTag("Ballz")) // Orbz don't have rbs
        {
            Rigidbody thisRb = thisBall.GetComponent<Rigidbody>();

            thisRb.AddForce(Random.Range(0f, 10f) * thisRb.mass * Vector3.back, ForceMode.Impulse);
            thisRb.AddTorque(new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)) * thisRb.mass, ForceMode.Impulse);
        }
    }

    BallStruct[] ballz;
    [SerializeField] GameObject[] editThingsToSpawn;
    [SerializeField] float[] editOddz;

    void Awake()
    {
        instance = this;
        currentCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        InitializeBallz();
    }

    private void Update()
    {
        if (!gameOver)
        {
            UIManagerMain.instance.UpdateTime();
            UIManagerMain.instance.UpdateSpellRecharge();
        }
    }

    private void InitializeBallz()
    {
        int arrayLength = editThingsToSpawn.Length;
        ballz = new BallStruct[arrayLength];
        float totalOddz = 0.0f;
        float cumulOddz = 0.0f;

        for (int n = 0; n < arrayLength; n++)
        {
            ballz[n] = new BallStruct(editThingsToSpawn[n], editOddz[n]);
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
    
    
}
