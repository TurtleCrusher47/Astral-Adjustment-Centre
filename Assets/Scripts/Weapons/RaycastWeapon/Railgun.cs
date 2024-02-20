using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : RaycastRangedWeapon
{
    protected override void UseSecondary()
    {
    }

    protected override void UseSecondaryUp()
    {
        Debug.Log("RB Up");
    }

    protected override void UseAbility()
    {
    }

    protected override void OnSecondary()
    {
    }

    protected override void OnAbility()
    {
    }
}
