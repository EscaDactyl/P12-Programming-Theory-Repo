using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Serializable Parameters    
    [SerializeField] float lateralSpeed = 4.8f; // probably make it the width of the track
    [SerializeField] float lateralBound = 4.4f; // probably make it the width of the track
    [SerializeField] float minRunSpeed = 7.2f;
    [SerializeField] float maxRunSpeed = 14.4f;

    // objects used throughout class
    Animator playerAnim;
    Rigidbody playerRb;

    // instance so that this can be referenced
    public static PlayerController instance;

    // visible player position to other scripts
    public Vector3 playerPos { get; private set; }
    public float currentRunSpeed;


    // A particle explosion before making the player die
    [SerializeField] ParticleSystem smokePfx;
    [SerializeField] ParticleSystem sparksPfx;
    [SerializeField] ParticleSystem firePfx;
    [SerializeField] GameObject explosionPfxObj;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentRunSpeed = minRunSpeed;
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.gameOver)
        {
            float horizInput = Input.GetAxis("Horizontal");
            float vertInput = Input.GetAxis("Vertical");

            if (horizInput != 0)
            {
                MovePlayer(horizInput);
            }

            if (vertInput != 0)
            {
                AdjustPlayerSpeed(vertInput);
            }

            transform.Translate(currentRunSpeed * Time.deltaTime * Vector3.forward);

            CheckForGrounding();

            UpdatePlayerGamePosition();
        }
    }

    public void MovePlayer(float horizInput)
    {
        transform.Translate(horizInput * lateralSpeed * Time.deltaTime * Vector3.right);
        if(transform.position.x > lateralBound)
        {
            transform.Translate((transform.position.x - lateralBound) * Vector3.left);
        }
        else if (transform.position.x < -lateralBound)
        {
            transform.Translate((transform.position.x + lateralBound) * Vector3.left);
        }
    }

    public void AdjustPlayerSpeed(float vertInput)
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

    // The game position might not be the actual position, especially when the player dies
    private void UpdatePlayerGamePosition()
    {
        playerPos = transform.position;
    }

    private void CheckForGrounding()
    {
        bool isGrounded = true;

        Collider playerCollider = GetComponentInChildren<CapsuleCollider>();
        // bool isGrounded = Physics.BoxCast(playerCollider.bounds.center, playerCollider.bounds.extents, Vector3.down, transform.rotation, Mathf.Infinity);
        if (transform.position.y < -10f)
            isGrounded = false;

        if(!isGrounded)
        {
            GameManager.instance.gameOver = true;
            StartCoroutine(PlayerFallToDoom());
        }
    }

    public IEnumerator KillPlayer()
    {
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

        // Camera shakes for one second
        phaseTime = 0.5f;
        timeElapsed = 0.0f;
        Vector3 origCameraPos = GameManager.instance.currentCamera.transform.position;
        Quaternion origCameraRot = GameManager.instance.currentCamera.transform.rotation;

        while (timeElapsed < phaseTime)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            GameManager.instance.currentCamera.transform.position = origCameraPos + new Vector3(Random.Range(-.05f, .05f), Random.Range(-.05f, .05f), Random.Range(-.05f, .05f));
            GameManager.instance.currentCamera.transform.rotation = Quaternion.Euler(origCameraRot.eulerAngles + new Vector3(Random.Range(-9f, 9f), Random.Range(-9f, 9f), Random.Range(-9f, 9f)));
        }

        // Set it back to the original
        GameManager.instance.currentCamera.transform.SetPositionAndRotation(origCameraPos,origCameraRot);
    }

    public IEnumerator PlayerFallToDoom()
    {
        float phaseTime = 1.0f;
        float timeElapsed = 0.0f;
        Vector3 startingCameraPos = GameManager.instance.currentCamera.transform.position;
        Vector3 finalCameraPos = playerPos;

        playerAnim.SetTrigger("isFallingT");

        while (timeElapsed < phaseTime)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            GameManager.instance.currentCamera.transform.position = Vector3.Lerp(startingCameraPos, finalCameraPos, timeElapsed / phaseTime);
            GameManager.instance.currentCamera.transform.LookAt(transform);
        }

        phaseTime = 2.0f;
        timeElapsed = 0.0f;
        smokePfx.gameObject.SetActive(true);

        while (timeElapsed < phaseTime)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            GameManager.instance.currentCamera.transform.LookAt(transform);
        }

        phaseTime = 2.0f;
        timeElapsed = 0.0f;
        sparksPfx.gameObject.SetActive(true);

        while (timeElapsed < phaseTime)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            GameManager.instance.currentCamera.transform.LookAt(transform);
        }

        phaseTime = 2.0f;
        timeElapsed = 0.0f;
        firePfx.gameObject.SetActive(true);

        while (timeElapsed < phaseTime)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            GameManager.instance.currentCamera.transform.LookAt(transform);
        }

        GameObject thisExplosion = Instantiate(explosionPfxObj, transform.position, explosionPfxObj.transform.rotation);
        thisExplosion.GetComponent<ParticleSystem>().Play();
        gameObject.SetActive(false);
    }
}
