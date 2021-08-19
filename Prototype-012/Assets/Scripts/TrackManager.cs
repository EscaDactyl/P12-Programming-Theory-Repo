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
        float trackLength = 10f; // There's probably a better way to do this?
        TrackSections = new GameObject[maxTrackSections];
        for(int n = 0; n < maxTrackSections; n++)
        {
            TrackSections[n] = Instantiate(trackPrefab, n * trackLength * Vector3.forward, trackPrefab.transform.rotation);
        }
    }

    private void RecycleTrack()
    {
        float farthestPosZ = 0;
        float trackLength = 10f;
        int needsToReplace = -1;

        for (int n = 0; n < maxTrackSections; n++)
        {
            if (TrackSections[n].transform.position.z > farthestPosZ)
            {
                farthestPosZ = TrackSections[n].transform.position.z;
            }
           if (PlayerController.instance.playerPos.z > TrackSections[n].transform.position.z + 1.5f * trackLength)
            {
                Destroy(TrackSections[n]);
                needsToReplace = n;
            }
        }

        if (needsToReplace > -1)
        {
            TrackSections[needsToReplace] = Instantiate(trackPrefab, (farthestPosZ + trackLength) * Vector3.forward, trackPrefab.transform.rotation);
        }

    }
}
