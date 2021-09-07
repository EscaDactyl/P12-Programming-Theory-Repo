using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class UIManagerMain : UIManagerParent
{
    // attributes to access
    public static UIManagerMain instance;

    // fields
    [SerializeField] TextMeshProUGUI distanceTMP;
    [SerializeField] TextMeshProUGUI scoreTMP;
    [SerializeField] TextMeshProUGUI spellTMP;
    [SerializeField] TextMeshProUGUI timeTMP;
    [SerializeField] Slider speedSliderMain;
    [SerializeField] Slider speedSliderMin;
    [SerializeField] Slider speedSliderMax;
    [SerializeField] Slider spellSlider;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Image backBlackImage;

    public bool CanCastSpell()
    {
        return (spellMeter == spellRecharge && !isCasting);
    }

    public void CastSpell(float spellLengthDelay)
    {
        // passes this to a Coroutine, which is hidden for abstraction
        isCasting = true;
        StartCoroutine(SpellRechargeDelay(spellLengthDelay));
        spellMeter = 0.0f;
        spellSlider.value = 0.0f;
    }

    public void FadeInStart()
    {
        StartCoroutine(FadeColor(blackImage, 1.0f, 0.0f, 2.0f));
    }

    public IEnumerator LaunchGameOverMenu()
    {
        float gameOverFadeTime = 3.0f;

        backBlackImage.gameObject.SetActive(true);
        gameOverPanel.SetActive(true);

        StartCoroutine(FadeColor(backBlackImage, 0.0f, 0.8f, gameOverFadeTime));

        for (int n = 0; n < gameOverPanel.transform.childCount; n++) // turns on every one of the children of game over panel
        {
            StartCoroutine(FadeColor(gameOverPanel.transform.GetChild(n).gameObject, 0.0f, 1.0f, gameOverFadeTime));
        }

        yield return new WaitForSeconds(gameOverFadeTime);
        cursor.SetActive(true);
        GameManager.instance.gameOver = true;
    }

    public void NewSpell(string spellName, Color sliderColor, float newSpellRecharge)
    {
        spellRecharge = newSpellRecharge;
        spellMeter = newSpellRecharge;
        UpdateSpellRecharge();
        spellTMP.SetText("Current Spell: " + spellName);

        Color newColor = sliderColor;
        newColor.a = 0.75f;

        spellSlider.value = 1.0f;
        spellSlider.fillRect.GetComponent<Image>().color = newColor;
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void StartOver()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void UpdateDistance(float distanceToAdd)
    {
        distance += distanceToAdd;
        distanceTMP.SetText("Distance: " + (distance / 1000).ToString("N2") + "km");
    }

    public void UpdateScore(float scoreToAdd)
    {
        score += scoreToAdd;
        scoreTMP.SetText("Score: " + score.ToString("N0"));
    }

    public void UpdateSpeed(float minAllowedSpeed, float speed, float maxAllowedSpeed, float maxSpeed)
    {
        speedSliderMain.value = speed / maxSpeed;
        speedSliderMin.value = minAllowedSpeed / maxSpeed;
        speedSliderMax.value = (maxSpeed - maxAllowedSpeed) / maxSpeed; // It's right-to-left
    }

    public void UpdateSpellRecharge()
    {
        // Does not increase during casting
        if (!isCasting)
        {
            spellMeter += Time.deltaTime;
        }
        
        if (spellMeter > spellRecharge)
        {
            spellMeter = spellRecharge;
        }
        spellSlider.value = spellMeter / spellRecharge;
    }

    public void UpdateTime()
    {
        timeElapsed += Time.deltaTime;
        string minutes = Mathf.FloorToInt(timeElapsed / 60).ToString("D2");
        string seconds = Mathf.FloorToInt(timeElapsed % 60).ToString("D2");
        string centiseconds = Mathf.FloorToInt(100 * (timeElapsed % 1)).ToString("D2");

        timeTMP.SetText("Time: " + minutes + "' " + seconds + "\" " + centiseconds);
    }

    // private attributes
    float score;
    float distance;
    float timeElapsed;
    float spellMeter;
    float spellRecharge;
    bool isCasting = false;


    private void Update()
    {
        // only valid if the game is over
        if(GameManager.instance.gameOver)
        {
            VertInput();
            CheckForSelection();
        }
    }

    protected override void UISceneAwake()
    {
        instance = this;
    }


    IEnumerator SpellRechargeDelay(float spellLengthDelay)
    {
        yield return new WaitForSeconds(spellLengthDelay);
        isCasting = false;
    }

}
