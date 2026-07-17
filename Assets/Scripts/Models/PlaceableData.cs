using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "PlacableData", menuName = "DataObjects/PlacableData")]
namespace Models
{
    [Serializable]
    public class PlaceableData // : ScriptableObject
    {
        //may be replaced by a hand craft tile system if we want L shaped objects (could do multi-d-array of boolean)
        public Vector3Int Size = Vector3Int.one;
        public string ResourceFileLocation; //this is the location of the Placeable Scriptible Object
        public int Rotation { get; set; } //clockwise rotation starting with front axis along x axis (then -z then -x then z)
        public Color Color = Color.white;
        public PlacableType PlacableType = PlacableType.Normal;

        public PlaceableData(int rotation = 0)
        {
            Rotation = rotation;
        }
        
        public PlaceableData(PlaceableData pd)
        {
            this.Size = pd.Size;
            this.ResourceFileLocation = pd.ResourceFileLocation;
            this.Rotation = pd.Rotation;
            this.Color = pd.Color;
            this.PlacableType = pd.PlacableType;
        }
        
    }
}