using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GridControllers;
using Managers;
using Models;
using UnityEngine;
using Grid = GridControllers.Grid;
using Object = System.Object;

namespace BuildControllers
{
    public class RoomController
    {
        // room grids job is to help reference the roomData to the references of the live objects that we spawn due to the roomData (model -> view middle man)
        //private RoomGrid _roomGrid;

        // we spawn an object in from model, that model has a view (a game object) when we manipulate that from user input via the controller, 
        // we need to know what part of the model to change, then we update our view.
        // data should be a reference to the data store in memory data
        private readonly RoomData _roomData;
        public HotelController ParentHotelController { get;}
        public Grid Grid { get;}
        private Room _room;

        public RoomController(RoomData rd, HotelController hc)
        {
            _roomData = rd;
            ParentHotelController = hc;
            Grid = new Grid(_roomData.RoomSize);
        }

        //Spawn in the room and all placeables within
        public void Init(Vector3 pos, Transform parentObject)
        {
            //init room
            GameMainManager.Instance.EventManager.StartListening(EventManager.EventName.RoomSelectedForEditing.ToString(), CheckToSeeIfAboveSelectedRoomAndDisableVisualsIfSo);
            GameMainManager.Instance.EventManager.StartListening(EventManager.EventName.RoomDeselected.ToString(), ARoomWasDeselectedSoEnsureShowingVisuals);
            //get the Scriptible Object first
            PlaceableDefaultInfo defaultInfo = GameMainManager.Instance.AssetBundleManager.LoadObject(_roomData.ResourceFileLocation) as PlaceableDefaultInfo;
            defaultInfo.AssetBundlePath = _roomData.ResourceFileLocation;
            GameObject roomGameObject = GameMainManager.Instance.ObjectPoolManager.LoadObject(defaultInfo);
            roomGameObject.transform.parent = parentObject;
            _room = roomGameObject.GetComponent<Room>();
            _room.Init(this);
            _room.transform.position = pos;

            //init internal placables
            foreach (KeyValuePair<PlaceableData, Vector3Int> kvp in _roomData.ItemOrigins)
            {
                //get the game object info first from the Placeable_SO
                PlaceableDefaultInfo placeableDefaultInfo = GameMainManager.Instance.AssetBundleManager.LoadObject(kvp.Key.ResourceFileLocation) as PlaceableDefaultInfo;
                //load an object, get the placable controller, do whatever we need to do to it, then call the init of the room passing it the room data and this
                placeableDefaultInfo.AssetBundlePath = kvp.Key.ResourceFileLocation;
                GameObject placableGameObject = GameMainManager.Instance.ObjectPoolManager.LoadObject(placeableDefaultInfo);
                placableGameObject.transform.parent = _room.VisualsHider.transform;
                Placeable p = placableGameObject.GetComponent<Placeable>();
                p.InitWithReference(this, placeableDefaultInfo, kvp.Key);
                //THIS IS VIEW WORK! add to view (soon)
                p.transform.position = GetWorldSpaceBasedOnGridPos(GetPosInHotel(), kvp.Value);
                //add the reference to the placeable object?
                GridOperator.AddOrUpdatePlaceable(p, kvp.Value, Grid);
            }
            //turn off all the colliders unless we are editing this room
            DisableAllPlaceableCollision(GetPlaceables());
        }

        public bool AddOrUpdatePlaceable(Placeable p, Vector3Int pos)
        {
            if (!GridOperator.AddOrUpdatePlaceable(p, pos, Grid)) return false;
            SaveModel();
            return true;
        }

        public bool GetModelOriginOfPlacable(Placeable p, out Vector3Int v3)
        {
             return Grid.placablesOrigins.TryGetByValue(p, out v3);
        }

        public void SaveModel()
        {
            //generate roomData
            _roomData.ItemOrigins.Clear();
            foreach (KeyValuePair<Placeable,Vector3Int> keyValuePair in Grid.placablesOrigins.AsDictionaryKey1())
            {
                _roomData.ItemOrigins.Add(new KeyValuePair<PlaceableData, Vector3Int>(keyValuePair.Key.PlaceableData, keyValuePair.Value));    
            }
            HotelController.SaveModel();
        }

        private void CheckToSeeIfAboveSelectedRoomAndDisableVisualsIfSo(object sender, EventArgs ea)
        {
            Vector3Int x = ((RoomSelectedEventArgs)ea).RoomController.GetPosInHotel();
            if (GetPosInHotel().y > x.y)
            {
                HideVisualsOfRoom(this);
            }
        }

        private void ARoomWasDeselectedSoEnsureShowingVisuals(object sender, EventArgs ea)
        {
            UnHideVisualsOfRoom(this);
        }

        public Vector3Int GetNearistSquarePosFromPos(Vector3 pos, Vector3Int size)
        {
            return NearistSquareCenter(pos);
        }

        //returns if it was able to remove the placeable
        public bool RemovePlaceable(Placeable p)
        {
            bool returnVal = GridOperator.RemovePlaceable(p, Grid);
            SaveModel();
            return returnVal;
        }

        public IEnumerable<Placeable> GetPlaceables()
        {
            return Grid.placablesOrigins.Keys1();
        }

        public bool GetPlaceableLocation(Placeable p, out Vector3 pos)
        {
            Vector3Int gridPos;
            if (!Grid.placablesOrigins.TryGetByValue(p, out gridPos))
            {
                pos = Vector3.zero;
                return false;
            }
            pos = GetWorldSpaceBasedOnGridPos(_room.transform.position, gridPos);
            return true;
        }

        public Dictionary<Placeable, Vector3Int> GetPlaceableLocations()
        {
            return Grid.placablesOrigins.AsDictionaryKey1();
        }

        public Vector3Int GetPosInHotel()
        {
            return ParentHotelController.GetRoomPos(this);
        }

        public static bool FindGridSpotForPlacableData(PlaceableData pd, Grid grid, out Vector3Int v3)
        {
            return GridOperator.FindSpotForPlacableData(pd, grid, out v3);
        }


        public static bool CheckIfSpotsAreAvalable(PlaceableData pd, Vector3Int gridPos, Grid _grid, Placeable p = null)
        {
            return GridOperator.CheckIfSpotsAreAvalable(Placeable.SpotsTakenOnTheGridBasedOffPos(pd, gridPos), _grid, p);
        }

        public static void DisableAllPlaceableCollision(IEnumerable<Placeable> placeables, IEnumerable<Placeable> exceptThese)
        {
            DisableAllPlaceableCollision(placeables.Except(exceptThese));
        }
        
        public static void DisableAllPlaceableCollision(IEnumerable<Placeable> placeables)
        {
            foreach (Placeable placeable in placeables)
            {
                placeable.DisableColliders();
            }
        }

        public static void EnableAllPlaceableCollision(IEnumerable<Placeable> placeables)
        {
            foreach (Placeable placeable in placeables)
            {
                placeable.EnableColliders();
            }
        }

        public static Vector3 GetWorldSpaceBasedOnGridPos(Vector3 roomPos, Vector3Int currentlySelectedGridPos)
        {
            return roomPos + currentlySelectedGridPos;
        }

        public static Vector3Int NearistSquareCenter(Vector3 v3)
        {
            return new Vector3Int(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.y), Mathf.RoundToInt(v3.z));
        }

        public static Vector3Int NearistSquareCenterMinusSize(Vector3 v3, Vector3 size)
        {
            return NearistSquareCenter(new Vector3(Mathf.RoundToInt(v3.x - size.x / 2), Mathf.RoundToInt(v3.y - size.y / 2),
                Mathf.RoundToInt(v3.z)));
        }

        public static Vector3Int GetNearistGridSpaceFromPos(Vector3 hitPoint, Vector3Int roomPos)
        {
            return NearistSquareCenter(hitPoint) - roomPos;
        }

        public static void Deselected(RoomController rc)
        {
            DisableAllPlaceableCollision(rc.GetPlaceables());
            rc._room.DisableRoomColliders(rc._room.BoxColliders);
            rc._room.GridPlaneRenderer.enabled = false;
            GameMainManager.Instance.EventManager.TriggerEvent(rc, EventManager.EventName.RoomDeselected.ToString(), new RoomSelectedEventArgs(rc));
        }

        public static void Selected(RoomController rc)
        {
            EnableAllPlaceableCollision(rc.GetPlaceables());
            rc._room.EnableRoomColliders(rc._room.BoxColliders);
            rc._room.GridPlaneRenderer.enabled = true;
            GameMainManager.Instance.EventManager.TriggerEvent(rc, EventManager.EventName.RoomSelectedForEditing.ToString(), new RoomSelectedEventArgs(rc));
        }

        public Vector3 GetWorldPosition()
        {
           return _room.transform.position;
        }

        //makes the room invisible
        public void HideVisualsOfRoom(RoomController rc)
        {
            _room.VisualsHider.Hide();
        }
        
        public void UnHideVisualsOfRoom(RoomController rc)
        {
            _room.VisualsHider.Unhide();
        }
    }
}