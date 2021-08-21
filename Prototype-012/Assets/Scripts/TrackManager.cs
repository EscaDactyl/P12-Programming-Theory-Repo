using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrackManager : MonoBehaviour
{
    // Adjustable parameter
    [SerializeField] int maxTrackSections;

    // Prefab objects
    [SerializeField] GameObject trackPrefab;

    // 

    // get-only object declarations
    public GameObject[] TrackSections { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        // track sections cannot be less than 10. this functionality will be moved to GameManager once I'm ready to do that.
        if(maxTrackSections < 10)
        {
            Debug.LogError("maxTrackSections too small, will not work");
            // Some sort of MainManager exitGame sequence
        }

        // Create track section arraw
        CreateTrack();
    }

    // Update is called once per frame
    void Update()
    {
        RecycleTrack();
    }

    private void CreateTrack()
    {
        float trackLength = trackPrefab.GetComponent<TrackPieceManager>().TrackLength;

        TrackSections = new GameObject[maxTrackSections];
        for(int n = 0; n < maxTrackSections; n++)
        {
            TrackSections[n] = Instantiate(trackPrefab, n * trackLength * Vector3.forward, trackPrefab.transform.rotation);
            
            if(n > 9) // let the first 100m be nothing, but then spice it up
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

    private void RecycleTrack()
    {
        float trackLength = 10f;

        if (PlayerController.instance.playerPos.z > TrackSections[0].transform.position.z + 3f * trackLength)
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
}
