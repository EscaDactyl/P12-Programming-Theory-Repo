using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPieceManager : MonoBehaviour
{
    public struct TrackChild
    {
        public GameObject TrackSection { get; }
        public float EndYCoord { get; set;  }

        public TrackChild(GameObject trackSection, float endYCoord)
        {
            TrackSection = trackSection;
            EndYCoord = endYCoord;
        }
    }
    public TrackChild[] TrackChildren { get; private set; }

    // Backing fields
    const float backingTrackLength = 5.0f;
    const float backingTrackThickness = 0.25f;

    // read-only useful constants
    public float TrackLength
    {
        get { return backingTrackLength;  }
    }
    public float TrackThickness
    {
        get { return backingTrackThickness;  }
    }

    public int TrackChildCount
    {
        get { return TrackChildren.Length; }
    }

    void Awake()
    {
        TrackChildren = new TrackChild[transform.childCount];

        // Assign track pieces
        for(int n = 0; n < transform.childCount; n++)
        {
            TrackChildren[n] = new TrackChild(transform.GetChild(n).gameObject, 0.0f);
        }

    }

    public void RandomizeTrackChild(int index, float beginYCoord)
    {
        if(index < 0 || index > TrackChildren.Length - 1)
        {
            return;
        }

        GameObject trackPiece = TrackChildren[index].TrackSection;
        float centralYCoord = 0.0f;
        TrackChildren[index].EndYCoord = (beginYCoord + Random.Range(-3.75f, 3.75f) + centralYCoord) / 2;
        float deltaYCoord = TrackChildren[index].EndYCoord - beginYCoord;

        // TRIGONOMETRY TIME

        float hypotenuse = Mathf.Sqrt(TrackLength * TrackLength + deltaYCoord * deltaYCoord); // hypotenuse
        float newScaleZ = hypotenuse / TrackLength;
        float rotationAngle = Mathf.Rad2Deg * Mathf.Atan2(deltaYCoord, TrackLength);
        float newCenterY = beginYCoord + deltaYCoord / 2;

        // Scale according to the hypotenuse
        trackPiece.transform.localScale = new Vector3(1.0f, 1.0f, newScaleZ);

        // Rotate according to the negative angle
        trackPiece.transform.Rotate(-rotationAngle, 0f, 0f, Space.Self);

        // Move center to new center
        trackPiece.transform.localPosition = new Vector3(trackPiece.transform.localPosition.x, newCenterY, trackPiece.transform.localPosition.z);
    }

}
