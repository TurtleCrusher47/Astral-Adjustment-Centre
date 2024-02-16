using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : GameObjectRangedWeapon
{
    protected override void UseAbility()
    {
        if (CanUseAbility())
        {

        }
    }

    protected override void UseSecondary()
    {
        if (CanUseSecondary())
        {

        }
    }

    protected override void OnSecondary()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSkill()
    {
        throw new System.NotImplementedException();
    }
}
