using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public struct RoomData
    {
        //the vector3Int can be extended to a more intricate location in the future if needed (stackables/walls/etc)
        public List<KeyValuePair<PlaceableData, Vector3Int>> ItemOrigins; //x = depth, y = height, z = width
        public int RoomId;
        public string RoomName;
        public string ResourceFileLocation;
        public Vector3Int RoomSize; //x = depth, y = height, z = width
        public RoomType RoomType;
    }
}