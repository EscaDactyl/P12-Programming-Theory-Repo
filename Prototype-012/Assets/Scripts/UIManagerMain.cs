using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManagerMain : MonoBehaviour
{
    // attributes to access
    public UIManagerMain instance;
    public TextMeshProUGUI distanceTMP;
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI spellTMP;
    public TextMeshProUGUI timeTMP;
    public Slider spellSlider;

    // private object
    [SerializeField] Image screenBlack;

    void Awake()
    {
        instance = this;
    }

}
