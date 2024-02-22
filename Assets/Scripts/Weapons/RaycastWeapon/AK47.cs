using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47 : RaycastRangedWeapon
{
    protected override void OnAbility()
    {
    }

    protected override void OnPrimary()
    {
        AudioManager.Instance.PlaySFX("SFXAK47Shoot");
    }

    protected override void OnSecondary()
    {
    }

    protected override void UseAbility()
    {
    }

    protected override void UseSecondary()
    {
    }
}
