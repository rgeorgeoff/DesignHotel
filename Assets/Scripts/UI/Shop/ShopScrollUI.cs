using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class ShopScrollUI : MonoBehaviour
    {
        //public List<Item> ItemList { get; set; }
        public Transform contentPanel;
        public ShopScrollUI otherShop;
        public Text myGoldDisplay;

        public float gold = 20f;

//        // Use this for initialization
//        void Start()
//        {
//            RefreshDisplay();
//        }
//
//        void RefreshDisplay()
//        {
//            RemoveButtons();
//            AddButtons();
//        }
//
//        private void RemoveButtons()
//        {
//            while (contentPanel.childCount > 0)
//            {
//                GameObject toRemove = transform.GetChild(0).gameObject;
//                buttonObjectPool.ReturnObject(toRemove);
//            }
//        }
//
//        private void AddButtons()
//        {
//            for (int i = 0; i < ItemList.Count; i++)
//            {
//                Item item = ItemList[i];
//                GameObject newButton = buttonObjectPool.GetObject();
//                newButton.transform.SetParent(contentPanel);
//
//                ShopButton sampleButton = newButton.GetComponent<ShopButton>();
//                sampleButton.Setup(item, this);
//            }
//        }
//
//        public void TryTransferItemToOtherShop(Item item)
//        {
//            if (otherShop.gold >= item.price)
//            {
//                gold += item.price;
//                otherShop.gold -= item.price;
//
//                AddItem(item, otherShop);
//                RemoveItem(item, this);
//
//                RefreshDisplay();
//                otherShop.RefreshDisplay();
//                Debug.Log("enough gold");
//            }
//
//            Debug.Log("attempted");
//        }
//
//        void AddItem(Item itemToAdd, ShopScrollUI shopList)
//        {
//            shopList.ItemList.Add(itemToAdd);
//        }
//
//        private void RemoveItem(Item itemToRemove, ShopScrollUI shopList)
//        {
//            for (int i = shopList.ItemList.Count - 1; i >= 0; i--)
//            {
//                if (shopList.ItemList[i] == itemToRemove)
//                {
//                    shopList.ItemList.RemoveAt(i);
//                }
//            }
//        }
    }
}