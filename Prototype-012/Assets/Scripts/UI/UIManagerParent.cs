using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIManagerParent : MonoBehaviour
{
    // Public object characteristics
    public int buttonSelectionIndex = 0;

    public IEnumerator FadeColor(Image image, float beginAlpha, float endAlpha, float duration)
    {
        float timeElapsed = 0.0f;
        float deltaAlpha = endAlpha - beginAlpha;

        float objectRed = image.color.r;
        float objectGreen = image.color.g;
        float objectBlue = image.color.b;

        image.color = new Color(objectRed, objectBlue, objectGreen, beginAlpha);

        while (timeElapsed < duration)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            image.color = new Color(objectRed, objectGreen, objectBlue, beginAlpha + deltaAlpha * timeElapsed / duration);
        }
    }

    public IEnumerator FadeColor(TextMeshProUGUI tmp, float beginAlpha, float endAlpha, float duration)
    {
        float timeElapsed = 0.0f;
        float deltaAlpha = endAlpha - beginAlpha;

        float objectRed = tmp.color.r;
        float objectGreen = tmp.color.g;
        float objectBlue = tmp.color.b;

        tmp.color = new Color(objectRed, objectBlue, objectGreen, beginAlpha);

        while (timeElapsed < duration)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            tmp.color = new Color(objectRed, objectGreen, objectBlue, beginAlpha + deltaAlpha * timeElapsed / duration);
        }
    }

    public IEnumerator FadeColor(GameObject objectWithColor, float beginAlpha, float endAlpha, float duration)
    {
        yield return new WaitForSeconds(0); // a dummy line to keep the format of the method consistent

        // demonstrate allowable types through if else
        if (objectWithColor.GetComponent<Image>() != null)
        {
            StartCoroutine(FadeColor(objectWithColor.GetComponent<Image>(), beginAlpha, endAlpha, duration));
        }
        // demonstrate allowable types through if else
        else if (objectWithColor.GetComponent<TextMeshProUGUI>() != null)
        {
            StartCoroutine(FadeColor(objectWithColor.GetComponent<TextMeshProUGUI>(), beginAlpha, endAlpha, duration));
        }
        else
        {
            Debug.LogError("Illegal parameter " + objectWithColor.name + " passed into FadeColor");
        }

    }
    
    // Protected Characteristics
    [SerializeField] protected GameObject[] buttonSelectionArray;
    [SerializeField] protected GameObject cursor;

    [SerializeField] protected AudioClip cursorSfx;
    [SerializeField] protected AudioSource sceneAudio;

    [SerializeField] protected Image blackImage;

    protected bool axisDown = false;
    protected bool selectionMade = false;
    protected abstract void UISceneAwake();

    protected void VertInput()
    {
        float vertInput = Input.GetAxisRaw("Vertical");

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

    protected void CheckForSelection()
    {
        float selectionDuration = 3.0f;

        // basically any key except Cancel
        bool hasSelected = Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3") || Input.GetButtonDown("Submit");

        if (hasSelected && !selectionMade)
        {
            selectionMade = true;
            buttonSelectionArray[buttonSelectionIndex].GetComponent<SelectionItem>().RunMethodFromSelection(selectionDuration);
            StartCoroutine(FlickerSelection(selectionDuration));

            float currentBlackImageAlpha = blackImage.color.a;
            blackImage.transform.SetAsLastSibling();  // moves blackImage to the front
            StartCoroutine(FadeColor(blackImage, currentBlackImageAlpha, 1.0f, selectionDuration));

        }
    }

    protected IEnumerator FlickerSelection(float duration)
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

    protected void MoveCursor(int buttonIndex)
    {
        cursor.transform.position = new Vector3(cursor.transform.position.x, buttonSelectionArray[buttonIndex].transform.position.y);
    }


    // Private characteristics
    void Awake()
    {
        UISceneAwake();
    }


}
