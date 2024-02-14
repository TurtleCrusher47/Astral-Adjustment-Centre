using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckable 
{
    bool isAggroed { get; set; }
    bool isInStrikingDistance { get; set; }

    void SetAggroStatus(bool isAggroed);

    void SetStrikingDistanceBool(bool isInStrikingDistance);
}
