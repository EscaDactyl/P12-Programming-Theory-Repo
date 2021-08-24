using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbzDash : Orbz
{
    public override Color SetOrbColor()
    {
        return new Color(106f/255f,208f/255f,255f/255f, 1f);
    }

    public override float GetRecharge()
    {
        return 6f;
    }

    public override string GetOrbName()
    {
        return "Dash";
    }
    public override void CastSpell()
    {
        float dashTimeLeft = 0.5f;

        PlayerController.instance.dashSpeed = PlayerController.instance.minRunSpeed;
        PlayerController.instance.SetLight(2.0f,GetOrbColor());
        PlayerController.instance.SetDustEffect(true);

        StartCoroutine(CheckForEndDash(dashTimeLeft));
    }

    IEnumerator CheckForEndDash(float dashTimeLeft)
    {
        yield return new WaitForSeconds(dashTimeLeft);

        PlayerController.instance.dashSpeed = 0;
        PlayerController.instance.SetLight(0.0f, Color.clear);
        PlayerController.instance.SetDustEffect(false);
    }
}
