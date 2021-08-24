using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIManagerParent : MonoBehaviour
{
    [SerializeField] Image blackImage;

    void Awake()
    {
        blackImage.gameObject.SetActive(false);
        UISceneAwake();
    }

    protected abstract void UISceneAwake();

    public IEnumerator FadeScreen(float beginAlpha, float endAlpha, float duration)
    {

        float timeElapsed = 0.0f;
        float deltaAlpha = endAlpha - beginAlpha;

        blackImage.color = new Color(0.0f, 0.0f, 0.0f, beginAlpha);
        blackImage.gameObject.SetActive(true);

        while (timeElapsed < duration)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            blackImage.color = new Color(0.0f, 0.0f, 0.0f, beginAlpha + deltaAlpha * timeElapsed / duration);
        }
    }
}
