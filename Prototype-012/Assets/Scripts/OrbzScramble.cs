using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbzScramble : Orbz
{
    public override Color SetOrbColor()
    {
        return new Color(2f/255f,255f/255f,156f/255f,1f);
    }

    public override float GetRecharge()
    {
        return 18f;
    }

    public override string GetOrbName()
    {
        return "Scramble";
    }

    public override void CastSpell()
    {
        PlaySpellSound();
        float fxDelay = 2.0f;
        PlayerController.instance.SetLight(2.0f, GetOrbColor());
        TrackManager.instance.ScrambleTrack();
        StartCoroutine(CheckForEndFx(fxDelay));
    }
    IEnumerator CheckForEndFx(float fxDelay)
    {
        yield return new WaitForSeconds(fxDelay);
        PlayerController.instance.SetLight(0.0f, Color.clear);
    }

}
