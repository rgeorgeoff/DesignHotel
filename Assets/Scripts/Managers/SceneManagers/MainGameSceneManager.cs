using System;
using BuildControllers;
using UnityEngine;

//Scene Managers handle all init process of a specific scene.  This one handles the main game scene.  
namespace Managers.SceneManagers
{
    public class MainGameSceneManager : SceneManagerBase
    {
        public static string SceneName = "MainGameScene";
        public HotelController HotelController;
        public InventoryController<string> InventoryController;
        public PlaceableShopController PlaceableShopController;
        public DefaultPlaceableEditor DefaultPlaceableEditor;
        public Canvas WorldSpaceCanvas;
        public GameObject PlaceableFootprintDisplay;

        public override void Init()
        {
            //load user UI
            PlaceableShopController = new PlaceableShopController(this);
            DefaultPlaceableEditor = new DefaultPlaceableEditor(this);
            //load hotel/rooms
            HotelController = new HotelController();
            //then we load our hotel from our model (which may or may not be brought into memory yet from file store)
            HotelController.Init();
            
            //TEMP!!! to select room
//            Debug.LogWarning("THIS IS TEMP CODE! DONT LET IT SLIDE");
//            GameMainManager.Instance.EventManager.TriggerEvent(this, EventManager.EventName.RoomSelected.ToString(), new RoomSelectedEventArgs(HotelController.GetRoomControllers()[0]));
            
            //load player placables inventory (for whats displayed in the store)
            InventoryController = new InventoryController<string>("Placables");
            //load visitor duders

            //load ....

            GameMainManager.Instance.EventManager.TriggerEvent(this, EventManager.EventName.MainGameSceneLoaded.ToString(),
                EventArgs.Empty);
        }

        private void OnDestroy()
        {
            DefaultPlaceableEditor.Dispose();
        }
    }
}