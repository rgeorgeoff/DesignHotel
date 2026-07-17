using System;
using Managers;
using Models;
using UI.Shop;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace DefaultNamespace.Views
{
    public class PlaceableShopView : MonoBehaviour
    {
        public Button ShopButton;
        public Button InvisibleCloseButton;
        public Button CloseButton;
        public GameObject ShopUIParentObject;
        public ShopScrollUI ScrollUi;
        public GameObject ShopPlaceableButton;

        public void Init()
        {
            DisableShop();
            GameMainManager.Instance.EventManager.StartListening(EventManager.EventName.RoomSelectedForEditing.ToString(), RoomSelected);
            GameMainManager.Instance.EventManager.StartListening(EventManager.EventName.RoomDeselected.ToString(), RoomDeselected);
        }

        private void RoomSelected(Object sender, EventArgs e)
        {
            EnableShopButton();
        }
        
        private void RoomDeselected(Object sender, EventArgs e)
        {
            DisableShopButton();
        }

        public void EnableShop()
        {     
            ShopUIParentObject.gameObject.SetActive(true);
            DisableShopButton();
        }
        
        public void DisableShop()
        {
            ShopUIParentObject.gameObject.SetActive(false); 
        }

        public void DisableShopButton()
        {
            ShopButton.gameObject.SetActive(false);
        }
        
        public void EnableShopButton()
        {
            ShopButton.gameObject.SetActive(true);
        }
        

        public ShopButton AddButton(PlaceableDefaultInfo placeableDefaultInfo)
        {
            GameObject go = Instantiate(ShopPlaceableButton);
            go.transform.SetParent(ScrollUi.contentPanel);
            go.transform.SetAsLastSibling();
            ShopButton shopButton = go.GetComponent<ShopButton>();
            shopButton.Init(placeableDefaultInfo);
            return shopButton;
        }
    }
}