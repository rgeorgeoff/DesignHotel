using System.Collections.Generic;

namespace Models
{
    public class InventoryData<T>
    {
        //if this is generic, its likely just a dictionary of T and the quantity
        public Dictionary<T, int> ItemQuantities;
    }
}