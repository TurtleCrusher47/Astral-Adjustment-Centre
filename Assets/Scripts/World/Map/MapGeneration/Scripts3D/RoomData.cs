using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RoomData : ScriptableObject
{
    [SerializeField] public List<GameObject> ObjectsList = new List<GameObject>();
    [SerializeField] public List<Vector3> maxMinChanceList = new List<Vector3>();
}
