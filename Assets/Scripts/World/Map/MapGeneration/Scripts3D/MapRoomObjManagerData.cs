using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapRoomObjManagerData : ScriptableObject
{
    [SerializeField] public RoomData startRoomData;
    [SerializeField] public RoomData endRoomData;
    [SerializeField] public List<RoomData> contentRoomData;
}
