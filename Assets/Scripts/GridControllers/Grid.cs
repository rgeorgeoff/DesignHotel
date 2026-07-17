using System.Collections.Generic;
using System.Linq;
using BuildControllers;
using Models;
using UnityEngine;

namespace GridControllers
{
    
    public struct Grid
    {
        //only one placable entry
        internal readonly BiDictionary<Placeable, Vector3Int> placablesOrigins;

        //every space (contains multiple of the same placable if its size > 1x1
        internal readonly Dictionary<Vector3Int, Placeable> filledGridSpaces;

        internal Vector3 gridSize;

        public Grid(Vector3Int gridSize)
        {
            this.gridSize = gridSize;
            placablesOrigins = new BiDictionary<Placeable, Vector3Int>();
            filledGridSpaces = new Dictionary<Vector3Int, Placeable>();
        }
    }

    public static class GridOperator
    {
        public static bool AddOrUpdatePlaceable(Placeable p, Vector3Int pos, Grid grid)
        {
            //first check it can fit where we want to put it
            List<Vector3Int> spots = Placeable.SpotsTakenOnTheGridBasedOffPos(p.PlaceableData, pos);
            if (!CheckIfSpotsAreAvalable(spots, grid, p))
                return false;

            //remove from grid if it exists
            RemovePlaceable(p, grid);

            //add to grid in new spot
            foreach (Vector3Int vector3Int in spots)
            {
                grid.filledGridSpaces[vector3Int] = p;
            }

            //add to dictionary of placeables and origins
            grid.placablesOrigins.AddOrUpdate(p, pos);

            return true;
        }

        public static bool RemovePlaceable(Placeable p, Grid grid)
        {
            //if we dont have it just return
            if (!grid.placablesOrigins.Contains(p))
                return false;

            //remove from grid
            List<Vector3Int> keysToUpdate =
                grid.filledGridSpaces.AsParallel().Where(kvp => kvp.Value == p).Select(x => x.Key).ToList();
            foreach (Vector3Int vector3Int in keysToUpdate)
            {
                grid.filledGridSpaces[vector3Int] = null;
            }

            grid.placablesOrigins.RemoveByKey(p);
            return true;
        }

        public static bool CheckIfSpotsAreAvalable(IEnumerable<Vector3Int> spots, Grid grid, Placeable placeableToIgnore = null)
        {
            //check is within the room
            if (!spots.All(v3i => v3i.x >= 0 && v3i.y >= 0 && v3i.z >= 0 &&
                                  v3i.x < grid.gridSize.x && v3i.y < grid.gridSize.y && v3i.z < grid.gridSize.z))
                return false;
            
            //check the taken spots
            return spots.All(v3i =>
                !grid.filledGridSpaces.ContainsKey(v3i) || grid.filledGridSpaces[v3i] == null ||
                grid.filledGridSpaces[v3i] == placeableToIgnore);
        }

        public static bool FindSpotForPlacable(Placeable p, Grid grid, out Vector3Int gridPos)
        {
            return FindSpotForPlacableData(p.PlaceableData, grid, out gridPos, p);
        }

        public static bool FindSpotForPlacableData(PlaceableData pd, Grid grid, out Vector3Int gridPos,
            Placeable p = null)
        {
            gridPos = Vector3Int.zero;
            //go through empty spots and for each one check against all spots that this piece needs, and if we get a true, go to the next spot.
            for (int rsx = 0; rsx < grid.gridSize.x; rsx++)
            {
                for (int rsz = 0; rsz < grid.gridSize.z; rsz++)
                {
                    gridPos.x = rsx;
                    gridPos.z = rsz;
                    if (CheckIfSpotsAreAvalable(Placeable.SpotsTakenOnTheGridBasedOffPos(pd, gridPos), grid, p))
                    {
                        return true;
                    }
                }
            }

            gridPos = Vector3Int.zero;
            return false;
        }
    }
}