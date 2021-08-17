using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStart : SelectionItem
{
    [SerializeField] AudioClip selectionSfx;
    [SerializeField] AudioSource sceneAudio;

    public override void RunMethodFromSelection(float duration)
    {
        sceneAudio.PlayOneShot(selectionSfx);
        StartCoroutine(DelayedRun(duration));
    }

    IEnumerator DelayedRun(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        UIManager.instance.StartGame();
        throw new System.NotImplementedException(); // It ain't implemented so sure
    }
}
