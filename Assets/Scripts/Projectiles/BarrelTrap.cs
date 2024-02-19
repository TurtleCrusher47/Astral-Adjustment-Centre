using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTrap : Trap
{
    protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == collidableLayers)
        {

        }
    }

    protected override void OnTrapTriggered()
    {
        throw new System.NotImplementedException();
    }

    protected override void TriggerTrap()
    {
        throw new System.NotImplementedException();
    }
}
