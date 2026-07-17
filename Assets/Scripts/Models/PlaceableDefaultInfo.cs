using UnityEngine;

namespace Models
{
    [CreateAssetMenu(fileName = "Placeable", menuName = "ScriptableObjects/Placeable")]
    public class PlaceableDefaultInfo: GameObjectAssetBundleInfo
    {
        public string Name;
        public int Cost;
        public Sprite ShopSprite;
        public PlaceableData DefaultPlaceableData;
    }
}