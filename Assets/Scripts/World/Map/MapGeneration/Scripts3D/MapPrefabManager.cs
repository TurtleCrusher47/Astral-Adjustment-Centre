using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPrefabManager : MonoBehaviour
{
    [SerializeField] public RoomData startRoomData;
    [SerializeField] public RoomData endRoomData;
    [SerializeField] public List<RoomData> contentRoomData;
}
