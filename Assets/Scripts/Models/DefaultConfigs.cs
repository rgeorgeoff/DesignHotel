using System.Collections.Generic;
using UnityEngine;

//these should be gotten from file location (or really just be in the file location)... this is basically only for testing and abstracting out of the classes for now
namespace Models
{
    public class DefaultConfigs
    {
        private readonly Dictionary<string, object> _configObjects = new Dictionary<string, object>();

        public DefaultConfigs()
        {
            HotelData buildingData = new HotelData();
            RoomData rd = new RoomData
            {
                RoomId = 0,
                RoomSize = new Vector3Int(16, 3, 16),
                RoomType = RoomType.Lobby,
                RoomName = "Lobby",
                ResourceFileLocation = "Assets/ScriptibleObjects/Rooms/room_ph_light.asset",
                ItemOrigins = new List<KeyValuePair<PlaceableData, Vector3Int>>()
            };
//            PlaceableData pd = new PlaceableData
//            {
//                Size = Vector3Int.one,
//                ResourceFileLocation = "Assets/ScriptibleObjects/Placeables/chair_ph.asset"
//            };
//            rd.ItemOrigins.Add(pd, new Vector3Int(0, 0, 0));
            buildingData.Rooms.Add(Vector3Int.zero, rd);
            RoomData rd2 = new RoomData
            {
                RoomId = 0,
                RoomSize = new Vector3Int(16, 3, 16),
                RoomType = RoomType.TestRoom,
                RoomName = "101",
                ResourceFileLocation = "Assets/ScriptibleObjects/Rooms/room_ph_light.asset",
                ItemOrigins = new List<KeyValuePair<PlaceableData, Vector3Int>>()
            };
            buildingData.Rooms.Add(new Vector3Int(0,11,0), rd2);
            _configObjects.Add("playersBuildingData", buildingData);
        }

        public T GetDefaultConfig<T>(string path)
        {
            object obj;
            _configObjects.TryGetValue(path, out obj);
            return (T) obj;
        }
        
        public static RoomData DefaultRoomConfig = new RoomData
        {
            RoomId = 0,
            RoomSize = new Vector3Int(16, 3, 16),
            RoomType = RoomType.TestRoom,
            RoomName = "Lobby",
            ResourceFileLocation = "Assets/ScriptibleObjects/Rooms/room_ph_light.asset",
            ItemOrigins = new List<KeyValuePair<PlaceableData, Vector3Int>>()
        };
    }
}