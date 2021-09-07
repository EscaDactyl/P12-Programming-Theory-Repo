using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbzBlast : Orbz
{
    public override Color SetOrbColor()
    {
        return new Color(255f/255f,22f/255f,53f/255f, 1f);
    }

    public override float GetRecharge()
    {
        return 6f;
    }

    public override string GetOrbName()
    {
        return "Blast";
    }
    public override void CastSpell()
    {
        float fxDelay = 1.0f;
        PlaySpellSound();
        PlayerController.instance.SetLight(2.0f, GetOrbColor());
        GameObject thisPfx = PlayerController.instance.InstantiateBlastEffect();
        StartCoroutine(CheckForEndFx(thisPfx, fxDelay));
    }

    IEnumerator CheckForEndFx(GameObject thisPfx, float fxDelay)
    {
        yield return new WaitForSeconds(fxDelay);
        PlayerController.instance.SetLight(0.0f, Color.clear);
        Destroy(thisPfx);
    }

}
