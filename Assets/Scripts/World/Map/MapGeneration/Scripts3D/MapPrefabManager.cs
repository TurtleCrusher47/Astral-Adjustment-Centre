using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPrefabManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> ObjectsList = new List<GameObject>();
    [SerializeField] public List<Vector3> maxMinChanceList = new List<Vector3>();
}
