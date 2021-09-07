using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSelectUnpause : SelectionItem
{
    [SerializeField] AudioClip selectionSfx;
    [SerializeField] AudioSource sceneAudio;

    public override void RunMethodFromSelection(float duration)
    {
        sceneAudio.PlayOneShot(selectionSfx);
        GameManager.instance.UnpauseGame();
    }

}
