using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    static GameData instance;

    void Awake()
    {
        if (instance != null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
