using System.Collections.Generic;
using System.Linq;
using Managers;
using Models;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace BuildControllers
{
    public class HotelController : IController
    {
        //get the model from model manager? - this is store -- but something needs to pull all that data to be initialized.
        //tell the view to update once we load in the model?

        //be able to notify the view when model changes.
        private const string ModelStoreName = "playersBuildingData";
        private const bool LoadTestData = true;
        private HotelData buildingData;

        //some knowledge of the model now
        public readonly Dictionary<Vector3Int, RoomController> RoomControllers = new Dictionary<Vector3Int, RoomController>();
        public GameObject parentObject;

        public void Init()
        {
            //load the model data and look at it to see what needs to be done next
            parentObject = new GameObject("HotelParentObject");
            buildingData = GameMainManager.Instance.DataStoreManager.Load<HotelData>(ModelStoreName);
            LoadModel(buildingData);
            //bring in a hotel view object and init (if a view is required)
        }

        public void LoadModel(HotelData buildingData)
        {
            //no file found, default to default data
            if (buildingData == null) 
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (LoadTestData)
                {
                    buildingData = GameMainManager.Instance.DefaultConfigs.GetDefaultConfig<HotelData>(ModelStoreName);
                    GameMainManager.Instance.DataStoreManager.Save(buildingData, ModelStoreName);
                }
            }

            // ~~Load Rooms~~
            //for each room in the model bring in a prefab and put it under an empty parent object
            
            foreach (KeyValuePair<Vector3Int, RoomData> kvp in buildingData.Rooms)
            {
                //load a room controller, do whatever we need to do to it, then call the init of the room passing it the room data and this 
                InitRoom(kvp.Key, kvp.Value);
            }
        }

        public void AddRoom(RoomData roomData, Vector3Int pos)
        {
            RoomController rc = new RoomController(roomData, this);
            RoomControllers.Add(pos, rc);
            rc.Init(pos, parentObject.transform);
        }
        
        public void AddRoomFromScratchAtPos(Vector3Int pos)
        {
            RoomData roomData = DefaultConfigs.DefaultRoomConfig;
            roomData.RoomName = GetRoomControllers().Count + "01";
            buildingData.Rooms.Add(pos, roomData);
            InitRoom(pos, roomData);
            SaveModel();
        }

        private void InitRoom(Vector3Int pos, RoomData roomData)
        {
            RoomController rc = new RoomController(roomData, this);
            RoomControllers.Add(pos, rc);
            rc.Init(pos, parentObject.transform);
        }

        public List<RoomController> GetRoomControllers()
        {
            return RoomControllers.Values.ToList();
        }
        
        public static void SaveModel()
        {
            GameMainManager.Instance.DataStoreManager.UpdatedModels(ModelStoreName);
        }

        public Vector3Int GetRoomPos(RoomController roomController)
        {
            return RoomControllers.FirstOrDefault(kvp => kvp.Value == roomController).Key;
        }

        public static RoomController GetClosestRoomTo(Vector3 pos, Dictionary<Vector3Int, RoomController> _roomControllers )
        {
            if (_roomControllers.Count <= 0)
                return null;
            return _roomControllers.OrderBy(x => Vector3.Distance(pos, x.Key)).FirstOrDefault().Value;
        }
        
    }
}