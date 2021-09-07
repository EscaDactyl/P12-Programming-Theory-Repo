using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;


public class UIManager : UIManagerParent
{
    // Public object characteristics
    public static UIManager instance;

    public void StartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void ViewHighScores()
    {
        // This isn't ready yet
        // SceneManager.LoadScene(3, LoadSceneMode.Single);
    }


    public void GameOptions()
    {
        // This isn't ready yet
        // SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
    public void ExitGame()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    protected override void UISceneAwake()
    {
        MoveCursor(buttonSelectionIndex);
        instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        VertInput();
        CheckForSelection();
    }



}
