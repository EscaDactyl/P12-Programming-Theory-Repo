using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerPause : UIManagerParent
{
    public static UIManagerPause instance;

    public void FadePause()
    {
        sceneAudio.PlayOneShot(pauseSfx);
        blackImage.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        blackImage.gameObject.SetActive(true);
        pausePanel.SetActive(true);
    }

    public void UnfadePause()
    {
        blackImage.color = Color.black;
        blackImage.gameObject.SetActive(false);
        pausePanel.SetActive(false);
    }

    protected override void UISceneAwake()
    {
        instance = this;
    }

    [SerializeField] GameObject pausePanel;
    [SerializeField] AudioClip pauseSfx;

    private void Update()
    {
        // Only valid if the game is paused
        if(GameManager.instance.gamePaused)
        {
            VertInput();
            CheckForSelection();
        }
    }
}
