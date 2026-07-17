using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Views;
using Managers.SceneManagers;
using Models;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class PlaceableShopController
    {
        private Canvas _mainCanvas;
        private PlaceableShopView _placeableShopView;
        private Dictionary<ShopButton, PlaceableDefaultInfo> buttonLookup = new Dictionary<ShopButton, PlaceableDefaultInfo>();
        
        
        public PlaceableShopController(SceneManagerBase smb)
        {
            _mainCanvas = smb.ScenesMainCanvas;
            _placeableShopView = _mainCanvas.GetComponent<PlaceableShopView>();
            _placeableShopView.Init();
            _placeableShopView.ShopButton.onClick.AddListener(OpenShopButtonClicked);
            _placeableShopView.InvisibleCloseButton.onClick.AddListener(ClickedOutsideShopButton);
            _placeableShopView.CloseButton.onClick.AddListener(ClickedOutsideShopButton);
            //load all items that are available? ... and for each button what is in the player inventory... etc
            InitButtons();
        }

        private void OpenShopButtonClicked()
        {
            _placeableShopView.EnableShop();
        }
        
        //will need a disable (clicking off screen at all)
        private void ClickedOutsideShopButton()
        {
            _placeableShopView.DisableShop();
            _placeableShopView.EnableShopButton();
        }

        private void InitButtons()
        {
            //for every Placable_SO we want to load...

            foreach (PlaceableDefaultInfo placeableDefaultInfo in GetPlacableList())
            {
                ShopButton shopButton = _placeableShopView.AddButton(placeableDefaultInfo); // attach method ands
                shopButton.OnClicked += ShopPlaceableButtonClicked;
                buttonLookup.Add(shopButton, placeableDefaultInfo);
            }
        }

        private void ShopPlaceableButtonClicked(PlaceableDefaultInfo placeableDefaultInfo)
        {
            //Send an object to edit mode manager as a new object with the info about where the request came from
            _placeableShopView.DisableShop();

            ((MainGameSceneManager) GameMainManager.Instance.CurrentSceneManager)
                                                       .DefaultPlaceableEditor
                                                       .AddPlaceableFromStore(HandleAttemtToAddObject, placeableDefaultInfo);
        }

        private void HandleAttemtToAddObject(bool placedSuccessfully)
        {
            _placeableShopView.EnableShop();
            if (placedSuccessfully)
            {
                
            }
        }
        
        private IEnumerable<PlaceableDefaultInfo> GetPlacableList()
        {
            //Hopefully this does not break because we are assuming here that every object in the placables bundle is a placeable_so
            //it would be cought in QA by just pulling up the store menu
            return GameMainManager.Instance.AssetBundleManager.GetAllObjectsInBundle(AssetBundleManager.KnownBundleNames.placeables.ToString()).Cast<PlaceableDefaultInfo>();
        }
    }
}