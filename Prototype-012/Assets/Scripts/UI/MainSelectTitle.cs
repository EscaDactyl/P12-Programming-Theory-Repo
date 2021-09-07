using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSelectTitle : SelectionItem
{
    [SerializeField] AudioClip selectionSfx;
    [SerializeField] AudioSource sceneAudio;

    public override void RunMethodFromSelection(float duration)
    {
        Time.timeScale = 1.0f; // It's possible to run this script from the pause menu
        sceneAudio.PlayOneShot(selectionSfx);
        StartCoroutine(DelayedRun(duration));
    }

    IEnumerator DelayedRun(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        UIManagerMain.instance.ReturnToTitle();
    }
}
