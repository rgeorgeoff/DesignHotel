using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class HotelData
    {
        public Dictionary<Vector3Int, RoomData> Rooms { get; set; }

        public HotelData()
        {
            Rooms = new Dictionary<Vector3Int, RoomData>();
        }
    }
}