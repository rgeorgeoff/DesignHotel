//using System.Collections.Generic;
//using Models;
//using UnityEngine;
//
//namespace BuildControllers
//{
//    public interface IRoomController
//    {
//        //bool RequestObjectMove(Placeable p, Vector3Int newPos); //just use add or update
//        //bool GetPlacableDataAtPos(Vector3Int pos, out Placeable p);
//        bool AddOrUpdatePlaceable(Placeable p, Vector3Int pos);
//        Vector3Int GetNearistSquarePosFromPos(Vector3 pos, Vector3Int size);
//        //bool CheckIfPosAvalable(Placeable p, Vector3Int pos);
//        bool RemovePlaceable(Placeable p);
//        void EnableAllPlaceableCollision(IEnumerable<Placeable> placeables);
//        void DisableAllPlaceableCollision(IEnumerable<Placeable> placeables);
//        Vector3 GetWorldSpaceBasedOnGridPos(Vector3Int gridPos);
//        Dictionary<Placeable, Vector3Int> GetPlaceableLocations();
//        Vector3Int GetPosInHotel();
//        bool CheckIfSpotsAreAvalable(PlaceableData pd, Vector3Int gridPos, Placeable p = null);
//    }
//}