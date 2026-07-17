using System;
using System.Collections.Generic;
using Managers;
using Managers.SceneManagers;
using Models;
using UnityEngine;

//When a piece is being edited these should bring up the arrows to the side and handle the rotation of the objects
//Also needs to know what placable is currently selected (if there is one)
//this guy handles the guys bought from the shop too, untill they are purchased and commited to the room data

//object is passed in and checks to see if it can move to a position in a room without commiting it (room functionality has to allow this)
namespace BuildControllers
{
    public class DefaultPlaceableEditor : IPlacableEditor
    {
        public Vector3Int CurrentlySelectedGridPos { get; private set; }
        public Placeable CurrentlySelected { get; set; }
        public RoomController CurrentlySelectedRoomController { get; set; }
        private Action<bool> onFinishedReturnSuccessOrFail;
        private ItemModifyWheel _itemModifyWheel;
        private bool _acceptingNewPlaceableToManage = true;
        private Ray _lastRay;
        private bool _useDisplace = true;
        private Vector3 _displaceV;
        private SenderLocation _placeableSenderLocation;
        private PlaceableFootprintDisplay _placeableFootprintDisplay;
        private GameObject _placeableFootprintDisplayGO;
        private List<PlaceableFootprintDisplay> _placeableFootprintDisplayPool = new List<PlaceableFootprintDisplay>();
        private Vector3Int _pickUpLocation;
        private int _pickUpRotation;
        
        private enum SenderLocation
        {
            Store,
            Room
        }
        
        public DefaultPlaceableEditor(SceneManagerBase smb)
        {
            //set up events for dopping and getting new placable
            GameMainManager.Instance.EventManager.StartListening(EventManager.EventName.PlacableSelected.ToString(),
                PlaceableSelectedEvent);
            
            GameMainManager.Instance.EventManager.StartListening(EventManager.EventName.ChangeRoomInFocus.ToString(),
                NewRoomFocusEvent);
            
            GameMainManager.Instance.EventManager.StartListening(EventManager.EventName.RoomDeselected.ToString(),
                RoomNoLongerBeingEdit);
            //Get reference to the Placeable UI objects, and reference button methods
            _itemModifyWheel = smb.ScenesMainCanvas.GetComponent<ItemModifyWheel>();
            _itemModifyWheel.ConfirmButton.onClick.AddListener(ConfirmPlaceObject);
            _itemModifyWheel.CancelButton.onClick.AddListener(CancelPlaceObject);
            _itemModifyWheel.RotateButton.onClick.AddListener(RotateClicked);
            if (smb.GetType() != typeof(MainGameSceneManager))
                Debug.LogError("SCENE MANAGER NOT EXPECTED TYPE FOR THIS COMPONENT");
            _placeableFootprintDisplayGO = UnityEngine.Object.Instantiate((smb as MainGameSceneManager)?.PlaceableFootprintDisplay);
            
        }

        public void PlaceableSelectedEvent(object sender, EventArgs ep)
        {
            PlacableSelectedEventArgs psea = ep as PlacableSelectedEventArgs;
            AssignPlaceableToManagerFromRoom(psea.Placable);
        }

        //idk why this is here, seems like if we switch rooms then we want to let go of whatever we are holding...
        //but the edit mode flow should take care of this.  and it should be listening to those events instead
        public void NewRoomFocusEvent(object sender, EventArgs ep)
        {
            if(CurrentlySelected != null)
                ConfirmPlaceObject();
        }

        private void AssignPlaceableToManagerFromRoom(Placeable p)
        {
            //if we already have a placable selected we should maybe ignore the incoming one?
            if (CurrentlySelected == p)
            {
                Debug.Log("already managing this selectable"); //can be removed in time.
                return;
            }
            if (!_acceptingNewPlaceableToManage)
            {
                Debug.Log("refused to accept new placeable to manage"); //can be removed in time.
                return;
            }
            //else
            _placeableSenderLocation = SenderLocation.Room;
            _pickUpLocation = p.ParentRoom.GetPlaceableLocations()[p];
            _pickUpRotation = p.PlaceableData.Rotation;
            DisownPlaceable();
            OwnPlaceable(p, _pickUpLocation);
        }

        private void DistroyPlaceable(Placeable placeable)
        {
            if (placeable != null)
            {
                if (_placeableFootprintDisplay)
                {
                    _placeableFootprintDisplay.RemoveFromPlaceable();
                    PutPlaceableFootprintBackIntoPool(_placeableFootprintDisplay);
                }
                RoomController.EnableAllPlaceableCollision(placeable.ParentRoom.GetPlaceables());
                //CurrentlySelected.transform.localScale = scale;
                placeable.OnMouseDown = null;
                placeable.OnMouseUp = null;
                placeable.OnMouseDrag = null;
                placeable.BeingMoved = false;
                placeable.ParentRoom.RemovePlaceable(placeable);
                UnityEngine.Object.Destroy(CurrentlySelected.gameObject);
            }
            _acceptingNewPlaceableToManage = true;
        }

        private void DisownPlaceable()
        {
            //if we currnetly own something, reset all the stuff
            if (CurrentlySelected != null)
            {
                if (_placeableFootprintDisplay)
                {
                    _placeableFootprintDisplay.RemoveFromPlaceable();
                    PutPlaceableFootprintBackIntoPool(_placeableFootprintDisplay);
                }
                RoomController.EnableAllPlaceableCollision(CurrentlySelected.ParentRoom.GetPlaceables());
                //CurrentlySelected.transform.localScale = scale;
                CurrentlySelected.OnMouseDown = null;
                CurrentlySelected.OnMouseUp = null;
                CurrentlySelected.OnMouseDrag = null;
                CurrentlySelected.BeingMoved = false;
                CurrentlySelected = null;
            }

            _acceptingNewPlaceableToManage = true;
        }

        public PlaceableFootprintDisplay GetPlaceableFootprintDisplay()
        {
            if (_placeableFootprintDisplayPool.Count > 0)
            {
                PlaceableFootprintDisplay pfd = _placeableFootprintDisplayPool[0];
                _placeableFootprintDisplayPool.RemoveAt(0);
                return pfd;
            }
            else
            {
                return UnityEngine.Object.Instantiate(_placeableFootprintDisplayGO)
                    .GetComponent<PlaceableFootprintDisplay>();
            }
        }
        
        private void PutPlaceableFootprintBackIntoPool(PlaceableFootprintDisplay placeableFootprintDisplay)
        {
            _placeableFootprintDisplayPool.Add(_placeableFootprintDisplay);
            _placeableFootprintDisplay = null;
        }

        private void RoomNoLongerBeingEdit(object sender, EventArgs e)
        {
            RoomController rc = ((RoomSelectedEventArgs) e).RoomController;
            if(CurrentlySelected != null)
                CancelPlaceObject();
            RoomController.DisableAllPlaceableCollision(rc.GetPlaceables());
        }

        private void OwnPlaceable(Placeable p, Vector3Int gridPos)
        {
            //set as our currently selected
            CurrentlySelected = p;
            
            //turn off accepting new placeables
            _acceptingNewPlaceableToManage = false;
            
            //set known grid pos in room before moving it
            CurrentlySelectedGridPos = gridPos;
            
            //set placeable to be movable
            CurrentlySelected.BeingMoved = true;
            
            //Add a placeable footprint display
            _placeableFootprintDisplay = GetPlaceableFootprintDisplay();
            _placeableFootprintDisplay.InitializeUnderPlaceable(p);
            
            //turn off other objects collision
            RoomController.DisableAllPlaceableCollision(CurrentlySelected.ParentRoom.GetPlaceables(), new List<Placeable>{CurrentlySelected});
            
            //temp change scale
            //scale = p.transform.localScale;
            //CurrentlySelected.transform.localScale = p.transform.localScale * 1.1f;
            
            //attach to hooks about being clicked/dragged/other collision based methods
            CurrentlySelected.OnMouseDown = PlaceableOnMouseDown;
            CurrentlySelected.OnMouseUp = PlaceableOnMouseUp;
            CurrentlySelected.OnMouseDrag = PlaceableOnMouseDrag;
            
            //turn on UI wheel
            _itemModifyWheel.EnableUI(CurrentlySelected.transform, p.IsPurchased);
        }
    

        //public delegate void PlaceingCompleted(bool placedSuccessfully);
        public void AddPlaceableFromStore(Action<bool> returnFunc, PlaceableDefaultInfo placeableDefaultInfo)
        {
            //record where we got this object from so we can handle what to do after placing
            _placeableSenderLocation = SenderLocation.Store;
            
            //link function
            onFinishedReturnSuccessOrFail = returnFunc;
            
            //first check to see if there is a place to bring in this object in the room we are working on, if so,
            //bring it in at that spot as a object held by this guy, and dont commit it to the room object until state
            //should change (payment is made, and acceptible location).
            Vector3Int spawnGridPos;
            if (!RoomController.FindGridSpotForPlacableData(placeableDefaultInfo.DefaultPlaceableData, 
                GameMainManager.Instance.RoomSelectionManager.CurrentlySelectedRoomController.Grid,
                out spawnGridPos))
            {
                Debug.Log("no space for the object to be bought");
                onFinishedReturnSuccessOrFail.Invoke(false);
                onFinishedReturnSuccessOrFail = null;
                return;
            }
            
            //spawn in the object then own it
            GameObject go = GameMainManager.Instance.ObjectPoolManager.LoadObject(placeableDefaultInfo);
            Placeable p = go.GetComponent<Placeable>();
            
            //initialize the placeable component with defaults
            if (p == null)
            {
                Debug.LogError("this object does not have a placeable component!");
                GameObject.Destroy(go);
                onFinishedReturnSuccessOrFail.Invoke(false);
                onFinishedReturnSuccessOrFail = null;
                return;
            }
            p.Init(GameMainManager.Instance.RoomSelectionManager.CurrentlySelectedRoomController,
                placeableDefaultInfo, placeableDefaultInfo.DefaultPlaceableData, false);
            CurrentlySelectedGridPos = spawnGridPos;
            p.transform.position = RoomController.GetWorldSpaceBasedOnGridPos(
                GameMainManager.Instance.RoomSelectionManager.CurrentlySelectedRoomController.GetPosInHotel(),
                spawnGridPos);
            OwnPlaceable(p, spawnGridPos);
        }

        public void ConfirmPlaceObject()
        {
            if(CurrentlySelected == null) return;
            //ensure we can place the item (its not currently in an invalid spot) 
            // and if we dont own it yet, check if money is large enough --
            // if this is the case, we keep the item on screen and the player can click cancel?
            //we should not even bring it into the room unless they can purchase it, BUT that status could change if they lose money while they are placing it. 
            
            //tell the placible to attempt to add to the room its assigned to or change position if already in the room
            if (!CurrentlySelected.ParentRoom.AddOrUpdatePlaceable(CurrentlySelected, CurrentlySelectedGridPos))
            {
                Debug.Log("unable to place here, conflict in move: " + CurrentlySelectedGridPos);
                //failed to add to room (put on same spot?)
                DistroyPlaceable(CurrentlySelected);
                CurrentlySelected = null;
                if (onFinishedReturnSuccessOrFail != null) onFinishedReturnSuccessOrFail.Invoke(false);
            }
            else
            {
                CurrentlySelected.IsPurchased = true;
                DisownPlaceable();
                //return event method to the caller of this placer if not null
                if (onFinishedReturnSuccessOrFail != null) onFinishedReturnSuccessOrFail.Invoke(true);
            }
            onFinishedReturnSuccessOrFail = null;
            _itemModifyWheel.DisableUI();    
            GameMainManager.Instance.EventManager.TriggerEvent(this, 
                EventManager.EventName.EnableCameraMovement.ToString(), EventArgs.Empty);
        }
        
        public void CancelPlaceObject()
        {
            //if it came from shop
            Debug.Log("here");
            _itemModifyWheel.DisableUI();
            ResetCurrentToPickUpLocation();
            DisownPlaceable();
            if (onFinishedReturnSuccessOrFail != null) onFinishedReturnSuccessOrFail.Invoke(false);
        }

        public void RotateClicked()
        {
            CurrentlySelected.RotateLeft();
            CheckValidPlacement();
        }
        
        internal void PlaceableOnMouseDown()
        {
            GameMainManager.Instance.EventManager.TriggerEvent(this,
                EventManager.EventName.DisableCameraMovement.ToString(), EventArgs.Empty);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit, 100.0f))
            {
                _lastRay.origin = CurrentlySelected.transform.position;
            }
            _displaceV = hit.point - CurrentlySelected.RotatePivotPoint.position;
        }
        
        internal void PlaceableOnMouseUp()
        {
            GameMainManager.Instance.EventManager.TriggerEvent(this, 
                EventManager.EventName.EnableCameraMovement.ToString(), EventArgs.Empty);
        }
        
        internal void PlaceableOnMouseDrag()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            CurrentlySelected._mouseRay.direction = ray.direction;
            CurrentlySelected._mouseRay.origin = ray.origin;
            if (_useDisplace)
            {
                ray.origin -= _displaceV; //new Vector3(0, _displaceY);
                CurrentlySelected._castRay = ray;
            }

            if (!Physics.Raycast(ray, out hit, 100.0f, 1 << LayerMask.NameToLayer("Grids"))) return;
            _lastRay.origin = hit.point;
            CurrentlySelectedGridPos = RoomController.GetNearistGridSpaceFromPos(hit.point, GameMainManager.Instance.RoomSelectionManager.CurrentlySelectedRoomController.GetPosInHotel());
            CurrentlySelected.transform.position = RoomController.GetWorldSpaceBasedOnGridPos(CurrentlySelected.ParentRoom.GetPosInHotel(), CurrentlySelectedGridPos);
            //ValidPlacmentViewUpdate();
            CheckValidPlacement();
        }

        private void CheckValidPlacement()
        {
            if (RoomController.CheckIfSpotsAreAvalable(CurrentlySelected.PlaceableData, CurrentlySelectedGridPos, CurrentlySelected.ParentRoom.Grid, CurrentlySelected))
            {
                foreach (Material meshRendererMaterial in CurrentlySelected.MeshRenderer.materials)
                {
                    //meshRendererMaterial.color = Color.white;
                    _placeableFootprintDisplay.ChangeStateToCompatable();
                }
            }
            else
            {
                foreach (Material meshRendererMaterial in CurrentlySelected.MeshRenderer.materials)
                {
                    _placeableFootprintDisplay.ChangeStateToIncompatable();
                }
            }
        }

        internal void UIMoveDrag()
        {
            
        }

        public void Dispose()
        {
            //seems like we are shutting down/closing out, best save the asset back to where it was.
            if (CurrentlySelected != null)
            {
                ResetCurrentToPickUpLocation();
            }
        }

        //this could be the store/inventory/etc
        public void ResetCurrentToPickUpLocation()
        {
            //add it to inventory or put it back in the room where it was.
            switch (_placeableSenderLocation)
            {
                case SenderLocation.Room:
                    CurrentlySelected.transform.position = _pickUpLocation;
                    CurrentlySelected.PlaceableData.Rotation = _pickUpRotation;
                    Placeable.RotateToRotationInt(_pickUpRotation, CurrentlySelected.RotatePivotPoint);
                    CurrentlySelected.ParentRoom.AddOrUpdatePlaceable(CurrentlySelected, _pickUpLocation);
                    break;
                case SenderLocation.Store:
                    //do nothing 
                    break;
            }
        }

        public void Init()
        {
        }
    }
}