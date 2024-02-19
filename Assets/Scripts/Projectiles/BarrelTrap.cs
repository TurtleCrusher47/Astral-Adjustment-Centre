using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTrap : Trap
{
    protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == collidableLayers && !isTriggered)
        {
            TriggerTrap();
        }
    }

    protected override void TriggerTrap()
    {
       Debug.Log("Triggered Trap");
    }

    protected override void OnTrapTriggered()
    {
        throw new System.NotImplementedException();
    }
}
