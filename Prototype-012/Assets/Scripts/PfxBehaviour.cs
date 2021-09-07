using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PfxBehaviour : MonoBehaviour
{
    public float selfDestructTime;
    [SerializeField] AudioClip particleSfx;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestruct());
        if (GetComponent<AudioSource>() != null && particleSfx != null)
        {
            GetComponent<AudioSource>().PlayOneShot(particleSfx);
        }
    }

    IEnumerator SelfDestruct()
    {
        float timeElapsed = 0.0f;

        while (timeElapsed < selfDestructTime)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
        }

        Destroy(gameObject);

    }

}
