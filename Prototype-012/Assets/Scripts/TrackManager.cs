using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(500)]
public class TrackManager : MonoBehaviour
{
    // make accessible from anywhere
    public static TrackManager instance;

    // get-only declarations
    public GameObject[] TrackSections { get; private set; }
    public float TrailingDistance { get { return constTrailingDistance; } }

    public void ScrambleTrack()
    {
        // Get z pos of the spot to be respawned
        float respawnZ = TrackSections[respawnIndex].transform.position.z;

        // Destroy all sections equal to or over the respawnIndex and do a splodey
        DestroyTrack(respawnIndex, maxTrackSections, true);

        // Build all sections equal to or over the respawnIndex
        CreateTrack(respawnIndex, maxTrackSections, respawnZ, false);
    }

    // Adjustable parameter
    [SerializeField] int maxTrackSections;

    // Prefab objects
    [SerializeField] GameObject trackPrefab;
    [SerializeField] GameObject scramblePfx;

    // Private variable
    float spawnZPos = 50f;
    float trackLength;

    // backing const
    const float constTrailingDistance = 115.2f;
    const int respawnIndex = 29;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        trackLength = trackPrefab.GetComponent<TrackPieceManager>().TrackLength;
        TrackSections = new GameObject[maxTrackSections];

        // track sections cannot be less than 10. this functionality will be moved to GameManager once I'm ready to do that.
        if (maxTrackSections < 10)
        {
            Debug.LogError("maxTrackSections too small, will not work");
            // Some sort of MainManager exitGame sequence
        }

        // Create track section array
        CreateTrack(0, maxTrackSections, -115, true);
    }

    // Update is called once per frame
    void Update()
    {
        FillTrackWithItems(trackLength); // This needs to be done a frame after everything is drawn but before the next recycle
        RecycleTrack();
    }

    private void CreateTrack(int minIndex, int maxIndex, float initialZ, bool isInitializing)
    {
        if (maxIndex > maxTrackSections)
        {
            Debug.LogError("TrackManager.CreateTrack() returned a higher maxIndex than maxTrackSections");
        }

        float forLoopInitialZ = initialZ - minIndex * trackLength;

        for (int n = minIndex; n < maxIndex; n++)
        {
            TrackSections[n] = Instantiate(trackPrefab, (forLoopInitialZ + n * trackLength) * Vector3.forward, trackPrefab.transform.rotation);

            if (!isInitializing || n > 33) // let the first 50m be nothing, but then spice it up
            {
                TrackPieceManager thisTrackPieceManager = TrackSections[n].GetComponent<TrackPieceManager>();
                TrackPieceManager prevTrackPieceManager = TrackSections[n - 1].GetComponent<TrackPieceManager>();
                for (int n2 = 0; n2 < thisTrackPieceManager.TrackChildren.Length; n2++)
                {
                    thisTrackPieceManager.RandomizeTrackChild(n2, prevTrackPieceManager.TrackChildren[n2].EndYCoord);
                }
            }
        }
    }

    private void DestroyTrack(int minIndex, int maxIndex, bool isScramble)
    {
        if (maxIndex > maxTrackSections)
        {
            Debug.LogError("TrackManager.DestroyTrack() returned a higher maxIndex than maxTrackSections");
        }

        for (int n = minIndex; n < maxIndex; n++)
        {
            if (isScramble) // Then splode
            {
                float lateralBounds = 4.0f; // There's probably a better way of adjusting lateral bounds programmatically, but it's also just a prototype
                float verticalbounds = 3.75f; // ditto
                float xPos = Random.Range(-lateralBounds, lateralBounds);
                float yPos = Random.Range(-verticalbounds, verticalbounds);
                float zPos = Random.Range(TrackSections[n].transform.position.z - trackLength / 2, TrackSections[n].transform.position.z + trackLength / 2);
                Instantiate(scramblePfx, new Vector3(xPos, yPos, zPos), scramblePfx.transform.rotation);
            }
            Destroy(TrackSections[n]);
        }
    }

    private void RecycleTrack()
    {

        if (PlayerController.instance.playerPos.z > TrackSections[0].transform.position.z + TrailingDistance)
        {
            Destroy(TrackSections[0]);
            for (int n = 1; n < maxTrackSections; n++)
            {
                TrackSections[n - 1] = TrackSections[n]; // Shift everything back one
            }

            // Probably smarter ways to do these, but we'll keep it here for now
            float farthestPosZ = TrackSections[maxTrackSections - 2].transform.position.z;

            TrackSections[maxTrackSections - 1] = Instantiate(trackPrefab, (farthestPosZ + trackLength) * Vector3.forward, trackPrefab.transform.rotation);
            TrackPieceManager thisTrackPieceManager = TrackSections[maxTrackSections - 1].GetComponent<TrackPieceManager>();
            TrackPieceManager prevTrackPieceManager = TrackSections[maxTrackSections - 2].GetComponent<TrackPieceManager>();
            for (int n2 = 0; n2 < thisTrackPieceManager.TrackChildren.Length; n2++)
            {
                thisTrackPieceManager.RandomizeTrackChild(n2, prevTrackPieceManager.TrackChildren[n2].EndYCoord);
            }
        }
    }

    private void FillTrackWithItems(float trackLength)
    {
        // Spawn items
        while (spawnZPos < TrackSections[maxTrackSections - 1].transform.position.z)
        {
            GameManager.instance.SpawnAThing(spawnZPos);
            spawnZPos += Random.Range(0f, trackLength * 2 / (spawnZPos / 1000 + 0.5f)); // First km is ~ 1 per 5m, gets to 15 per 5m at 15km
        }
    }
}
