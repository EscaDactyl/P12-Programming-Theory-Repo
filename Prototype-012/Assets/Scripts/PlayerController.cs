using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class PlayerController : MonoBehaviour
{
    // instance so that this can be referenced
    public static PlayerController instance;

    // visible player position to other scripts
    public Vector3 playerPos { get; private set; }
    public AudioSource playerAudio { get; private set; }
    public float currentRunSpeed;
    public float minRunSpeed = 7.2f;
    public float maxRunSpeed = 14.4f;
    public float dashSpeed = 0.0f;

    public void SetLight(float lightIntensity, Color lightColor)
    {
        playerLight.intensity = lightIntensity;
        playerLight.color = lightColor;
    }

    public GameObject InstantiateBlastEffect()
    {
        return Instantiate(blastPfxObj, transform.position, blastPfxObj.transform.rotation);
    }

    public GameObject InstantiateBurstEffect()
    {
        return Instantiate(burstPfxObj, transform.position, burstPfxObj.transform.rotation);
    }

    public void SetDustEffect(bool isOn)
    {
        dustPfx.gameObject.SetActive(isOn);
    }

    public IEnumerator KillPlayer()
    {
        playerAudio.PlayOneShot(hitSfx);

        float phaseTime = 1.0f;
        float timeElapsed = 0.0f;
        Vector3 startingCameraPos = GameManager.instance.currentCamera.transform.position;
        Vector3 finalCameraPos = startingCameraPos + new Vector3(0f,Random.Range(1f,10f),Random.Range(-10f,-5f));

        playerAnim.SetTrigger("isDeadT");
        playerRb.freezeRotation = false;
        
        while(timeElapsed < phaseTime)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            GameManager.instance.currentCamera.transform.position = Vector3.Lerp(startingCameraPos, finalCameraPos, timeElapsed / phaseTime);
            transform.position = Vector3.Lerp(playerPos, finalCameraPos + 1.0f * Vector3.forward, timeElapsed / phaseTime);
        }

        // This creates the player sliding down effect
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        // Camera shakes
        phaseTime = 0.5f;
        timeElapsed = 0.0f;
        Vector3 origCameraPos = GameManager.instance.currentCamera.transform.position;
        Quaternion origCameraRot = GameManager.instance.currentCamera.transform.rotation;
        playerAudio.PlayOneShot(cameraShakeSfx);

        while (timeElapsed < phaseTime)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            GameManager.instance.currentCamera.transform.position = origCameraPos + new Vector3(Random.Range(-.05f, .05f), Random.Range(-.05f, .05f), Random.Range(-.05f, .05f));
            GameManager.instance.currentCamera.transform.rotation = Quaternion.Euler(origCameraRot.eulerAngles + new Vector3(Random.Range(-9f, 9f), Random.Range(-9f, 9f), Random.Range(-9f, 9f)));
        }

        // Set it back to the original
        GameManager.instance.currentCamera.transform.SetPositionAndRotation(origCameraPos,origCameraRot);
        StartCoroutine(UIManagerMain.instance.LaunchGameOverMenu());
    }

    public IEnumerator PlayerFallToDoom()
    {
        float phaseTime = 0.5f;
        float timeElapsed = 0.0f;
        Vector3 startingCameraPos = GameManager.instance.currentCamera.transform.position;
        Vector3 finalCameraPos = playerPos;

        playerAnim.SetTrigger("isFallingT");
        playerAudio.PlayOneShot(fallingSfx);

        while (timeElapsed < phaseTime)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            GameManager.instance.currentCamera.transform.position = Vector3.Lerp(startingCameraPos, finalCameraPos, timeElapsed / phaseTime);
            GameManager.instance.currentCamera.transform.LookAt(transform);
        }

        phaseTime = 0.5f;
        timeElapsed = 0.0f;
        smokePfx.gameObject.SetActive(true);

        while (timeElapsed < phaseTime)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            GameManager.instance.currentCamera.transform.LookAt(transform);
        }

        phaseTime = 0.5f;
        timeElapsed = 0.0f;
        sparksPfx.gameObject.SetActive(true);

        while (timeElapsed < phaseTime)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            GameManager.instance.currentCamera.transform.LookAt(transform);
        }

        phaseTime = 0.5f;
        timeElapsed = 0.0f;
        firePfx.gameObject.SetActive(true);

        while (timeElapsed < phaseTime)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            GameManager.instance.currentCamera.transform.LookAt(transform);
        }

        playerAudio.PlayOneShot(splatSfx);
        GameObject thisExplosion = Instantiate(explosionPfxObj, transform.position, explosionPfxObj.transform.rotation);
        thisExplosion.GetComponent<ParticleSystem>().Play();
        phaseTime = 0.5f;
        playerBody.SetActive(false);
        yield return new WaitForSeconds(phaseTime);
        StartCoroutine(UIManagerMain.instance.LaunchGameOverMenu());
    }

    // consts that I'm not changing
    const float lateralSpeed = 4.0f; // It's about half the track now
    const float lateralBound = 4.0f;

    // private changeable value (until it's not)
    private float maxSpeed = 57.6f;

    // A particle explosion before making the player die
    [SerializeField] ParticleSystem dustPfx;
    [SerializeField] ParticleSystem smokePfx;
    [SerializeField] ParticleSystem sparksPfx;
    [SerializeField] ParticleSystem firePfx;
    [SerializeField] GameObject blastPfxObj;
    [SerializeField] GameObject burstPfxObj;
    [SerializeField] GameObject explosionPfxObj;
    [SerializeField] AudioClip hitSfx;
    [SerializeField] AudioClip powerUpSfx;
    [SerializeField] AudioClip cameraShakeSfx;
    [SerializeField] AudioClip fallingSfx;
    [SerializeField] AudioClip splatSfx;

    // Serializable Parameters    
    [SerializeField] Orbz heldOrbz;
    [SerializeField] GameObject auxSlot;
    [SerializeField] GameObject playerBody;

    // components used throughout class
    Animator playerAnim;
    Rigidbody playerRb;
    Light playerLight;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentRunSpeed = minRunSpeed;
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerLight = GetComponent<Light>();
        playerRb = GetComponent<Rigidbody>();
        heldOrbz.isPowerup = false;
        UIManagerMain.instance.NewSpell(heldOrbz.GetOrbName(), heldOrbz.GetOrbColor(), heldOrbz.GetRecharge());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameActive)
        {
            float horizInput = Input.GetAxis("Horizontal");
            float vertInput = Input.GetAxis("Vertical");
            bool castSpell = Input.GetButtonDown("Fire1");
            bool pauseGame = Input.GetButtonDown("Cancel");

            UpdatePlayerSpeedBounds();

            if (horizInput != 0)
            {
                MovePlayer(horizInput);
            }

            if (vertInput != 0)
            {
                AdjustPlayerSpeed(vertInput);
            }

            if (castSpell && UIManagerMain.instance.CanCastSpell()) // I probably should move this away from the UI but I'm lazy
            {
                StartCoroutine(SpellAnimationDelay());
            }

            if (pauseGame)
            {
                GameManager.instance.PauseGame();
            }

            transform.Translate((currentRunSpeed + dashSpeed) * Time.deltaTime * Vector3.forward);

            CheckForGrounding();

            UIManagerMain.instance.UpdateDistance(transform.position.z - playerPos.z); // Distance since last frame
            UIManagerMain.instance.UpdateScore((currentRunSpeed + dashSpeed) * (currentRunSpeed + dashSpeed) * Time.deltaTime); // Velocity squared down to frame
            UIManagerMain.instance.UpdateSpeed(minRunSpeed, (currentRunSpeed + dashSpeed), maxRunSpeed, maxSpeed);
            UpdatePlayerGamePosition(); // this updates the frame
        }
    }

    private void MovePlayer(float horizInput)
    {
        transform.Translate(horizInput * lateralSpeed * Time.deltaTime * Vector3.right);
        if (transform.position.x > lateralBound)
        {
            transform.Translate((transform.position.x - lateralBound) * Vector3.left);
        }
        else if (transform.position.x < -lateralBound)
        {
            transform.Translate((transform.position.x + lateralBound) * Vector3.left);
        }
    }

    private void AdjustPlayerSpeed(float vertInput)
    {
        float runSpeedRange = maxRunSpeed - minRunSpeed;

        // two seconds to go from min to max speed
        currentRunSpeed += vertInput * Time.deltaTime * runSpeedRange / 2.0f;
        if (currentRunSpeed > maxRunSpeed)
        {
            currentRunSpeed = maxRunSpeed;
        }
        else if (currentRunSpeed < minRunSpeed)
        {
            currentRunSpeed = minRunSpeed;
        }
    }

    private void UpdatePlayerSpeedBounds()
    {
        float maxSpeedIncreasePerMinute = 2.88f; // This is much more intuitive for difficulty adjustments than per second (2.88f = hit max at 15 minutes of play)

        minRunSpeed += maxSpeedIncreasePerMinute / 3 / 60 * Time.deltaTime; // Even though it starts at 1/2 of max speed, make it go up slower for more play
        maxRunSpeed += maxSpeedIncreasePerMinute / 60 * Time.deltaTime;

        // Bounding
        AdjustPlayerSpeed(0);
        if ((maxRunSpeed + minRunSpeed) > maxSpeed)
        {
            maxSpeed = maxRunSpeed + minRunSpeed;
        }
    }

    // The game position might not be the actual position, especially when the player dies
    private void UpdatePlayerGamePosition()
    {
        playerPos = transform.position;
    }

    private void CheckForGrounding()
    {
        float lethalHeight = -7.5f;

        Collider playerCollider = GetComponentInChildren<CapsuleCollider>();
        if (transform.position.y < lethalHeight)
        {
            GameManager.instance.gameActive = false;
            StartCoroutine(PlayerFallToDoom());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.instance.gameActive && other.CompareTag("Orbz"))
        {
            // DelayedDestroy tha current item to allow spells to finish
            StartCoroutine(DelayedDestroy(heldOrbz.gameObject));

            // Put the new item in
            heldOrbz = other.gameObject.GetComponent<Orbz>();
            heldOrbz.gameObject.transform.SetParent(auxSlot.transform);
            heldOrbz.gameObject.transform.localPosition = Vector3.zero;
            heldOrbz.gameObject.transform.localScale = 4.0f * Vector3.one;

            // Call the UI function
            UIManagerMain.instance.NewSpell(heldOrbz.GetOrbName(), heldOrbz.GetOrbColor(), heldOrbz.GetRecharge());

            // Score a thousand points
            UIManagerMain.instance.UpdateScore(1000);

            // Power up Sound!
            playerAudio.PlayOneShot(powerUpSfx);
        }
    }

    IEnumerator SpellAnimationDelay()
    {
        float spellDelay = 0.3f; // based on transitions from animator inspector

        UIManagerMain.instance.CastSpell(spellDelay);
        playerAnim.SetTrigger("isCastingT");
        yield return new WaitForSeconds(spellDelay);
        heldOrbz.CastSpell();
    }

    IEnumerator DelayedDestroy(GameObject thisObject)
    {
        float theLongestCoroutineTimeYouCanThinkOf = 0.6f; // Look, let's be self-explanatory here :P
        thisObject.SetActive(false);

        yield return new WaitForSeconds(theLongestCoroutineTimeYouCanThinkOf);

        Destroy(thisObject);
    }
}
