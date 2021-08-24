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
    public int buttonSelectionIndex = 0;

    public void MoveCursor(int buttonIndex)
    {
        cursor.transform.position = new Vector3(cursor.transform.position.x, buttonSelectionArray[buttonIndex].transform.position.y);
    }
    public void StartGame()
    {
        // This isn't ready yet
        // SceneManager.LoadScene(1, LoadSceneMode.Single);
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

    [SerializeField] GameObject[] buttonSelectionArray;
    [SerializeField] GameObject cursor;

    [SerializeField] AudioClip cursorSfx;
    [SerializeField] AudioSource sceneAudio;


    // Private object characteristics
    bool axisDown = false;
    bool selectionMade = false;

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

    private void VertInput()
    {
        float vertInput = Input.GetAxis("Vertical");

        if (vertInput != 0 && !axisDown && !selectionMade)
        {
            axisDown = true;
            sceneAudio.PlayOneShot(cursorSfx);
            if (vertInput > 0)
            {
                buttonSelectionIndex--;
                if (buttonSelectionIndex < 0)
                {
                    buttonSelectionIndex = buttonSelectionArray.Length - 1; // wrap
                }
            }
            else
            {
                buttonSelectionIndex++;
                if (buttonSelectionIndex >= buttonSelectionArray.Length)
                {
                    buttonSelectionIndex = 0; // wrap
                }
            }
        }
        else if (vertInput == 0)
        {
            axisDown = false;
        }

        MoveCursor(buttonSelectionIndex);
    }

    private void CheckForSelection()
    {
        float selectionDuration = 3.0f;

        // basically any key except Cancel
        bool hasSelected = Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3") || Input.GetButtonDown("Submit");

        if (hasSelected && !selectionMade)
        {
            selectionMade = true;
            buttonSelectionArray[buttonSelectionIndex].GetComponent<SelectionItem>().RunMethodFromSelection(selectionDuration);
            StartCoroutine(FlickerSelection(selectionDuration));
            StartCoroutine(FadeScreen(0.0f,1.0f,selectionDuration));
        }
    }

    private IEnumerator FlickerSelection(float duration)
    {
        float flickerDuration = 0.1f;
        float timeElapsed = 0.0f;

        while (timeElapsed < duration)
        {
            yield return new WaitForSecondsRealtime(flickerDuration);
            timeElapsed += flickerDuration;
            cursor.SetActive(!cursor.activeSelf);
        }
    }

}
