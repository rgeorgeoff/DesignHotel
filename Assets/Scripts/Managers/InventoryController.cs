using Models;

namespace Managers
{
    public class InventoryController<T>
    {
        private string _inventoryName;
        public InventoryData<T> InventoryData {get {return GameMainManager.Instance.DataStoreManager.Load<InventoryData<T>>(_inventoryName);}}
        
        public InventoryController(string inventoryName)
        {
            _inventoryName = inventoryName;
        }

        public void AddPlacableToInventory(T placeablePath, int qty)
        {
            if (InventoryData.ItemQuantities.ContainsKey(placeablePath))
                InventoryData.ItemQuantities[placeablePath] += qty;
            else
                InventoryData.ItemQuantities.Add(placeablePath, qty);
        }
        
        /// <summary>
        /// try to remove item from inventory
        /// </summary>
        /// <param name="placablePath"></param>
        /// <param name="qty"></param>
        /// <returns> false if we dont have the item, or we dont have enough, otherwise true</returns>
        public bool RemovePlacableFromInventory(T item, int qty)
        {
            if (!InventoryData.ItemQuantities.ContainsKey(item)) return false;
            if (InventoryData.ItemQuantities[item] < qty) return false;
            InventoryData.ItemQuantities[item] -= qty;
            return true;
        }

        public int GetQtyOfItem(T item)
        {
            int qty;
            InventoryData.ItemQuantities.TryGetValue(item, out qty);
            return qty;
        }
    }
}