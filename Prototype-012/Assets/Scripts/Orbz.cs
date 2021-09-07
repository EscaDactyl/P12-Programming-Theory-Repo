using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Orbz : MonoBehaviour
{
    // public
    public bool isPowerup = true;

    public virtual Color SetOrbColor()
    {
        return Color.white;
    }
    public Color GetOrbColor()
    {
        return SetOrbColor();
    }

    public abstract float GetRecharge();
    public abstract string GetOrbName();

    public abstract void CastSpell();

    // Editor object
    [SerializeField] AudioClip spellSfx;
    
    // Protected script variables
    protected float timeElapsed;
    protected Light baseOrbLight;
    protected Vector3 rotationDirection;

    // Protected script consts
    protected const float lightIntensity = 1.0f;
    protected const float lightPeriod = 1.0f;
    protected const float rotationPeriod = 1.0f;

    // Start is called before the first frame update
    protected void Awake()
    {
        baseOrbLight = GetComponent<Light>();
        baseOrbLight.color = SetOrbColor();
        rotationDirection = RandomizeRotation();
    }

    // Update is called once per frame
    protected void Update()
    {
        timeElapsed += Time.deltaTime;
        PulseLight();
        RotateOrb();
        CheckIfLeftBehind();
    }

    protected void PlaySpellSound()
    {
        PlayerController.instance.playerAudio.PlayOneShot(spellSfx);
    }

    protected void PulseLight()
    {
        baseOrbLight.intensity = Mathf.Sin(timeElapsed * (2 * Mathf.PI) / lightPeriod) * lightIntensity;
    }

    protected void RotateOrb()
    {
        transform.Rotate(360 * rotationDirection * Time.deltaTime / rotationPeriod);
    }

    protected Vector3 RandomizeRotation()
    {
        return (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized;
    }

    protected void CheckIfLeftBehind()
    {
        float backBound = PlayerController.instance.playerPos.z - TrackManager.instance.TrailingDistance;

        if (transform.position.z < backBound)
        {
            Destroy(gameObject);
        }

    }

}
