using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Serializable Parameters    
    [SerializeField] float lateralSpeed = 4.848f; // probably make it the width of the track

    // objects used throughout class
    Animator playerAnim;

    // instance so that this can be referenced
    public static PlayerController instance;

    // visible player position to other scripts
    public Vector3 playerPos { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizInput = Input.GetAxis("Horizontal");

        if(horizInput != 0)
        {
            MovePlayer(horizInput);
        }

        /* These are tests
        bool buttonFire1 = Input.GetButtonDown("Fire1");

        if(buttonFire1)
        {
            playerAnim.SetBool("isCastingB",true);
        }

        bool buttonJump = Input.GetButtonDown("Jump");

        if(buttonJump)
        {
            playerAnim.SetTrigger("isDeadT");
        }
        */

        transform.Translate(7.5f * Time.deltaTime * Vector3.forward);

        UpdatePlayerGamePosition();
    }

    public void MovePlayer(float horizInput)
    {
        transform.Translate(horizInput * lateralSpeed * Time.deltaTime * Vector3.right);
        // Future code: if (transform.position.x (magnitude) > horizontal limit according to the track piece, then move it back
        if(transform.position.x > 2.424f)
        {
            transform.Translate((transform.position.x - 2.424f) * Vector3.left);
        }
        else if (transform.position.x < -2.424f)
        {
            transform.Translate((transform.position.x + 2.424f) * Vector3.left);
        }
    }

    // The game position might not be the actual position, especially when the player dies
    private void UpdatePlayerGamePosition()
    {
        playerPos = transform.position;
    }
}
