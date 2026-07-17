using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;

namespace BuildControllers
{
    class Room : MonoBehaviour
    {
        //HotelController parentHotelView; //this should actually not be a view here, but the controller, since rooms will only interact upwards through hotel controller
        public RoomController RoomController { get; set; }
        private const string layer = "Grids";
        [HideInInspector]
        public List<BoxCollider> BoxColliders;
        [HideInInspector]
        public VisualsHider VisualsHider;

        public MeshRenderer GridPlaneRenderer;

        public void Init(RoomController myController)
        {
            RoomController = myController;
            int layerInt = LayerMask.NameToLayer(layer);
            SetLayerRecursively(gameObject, layerInt);
            BoxColliders = GetComponentsInChildren<BoxCollider>().ToList();
            VisualsHider = GetComponentInChildren<VisualsHider>();
            // ensure BoxColliders are off unless room is currently selected/being eidited ... may have to filter between wall
            // selection colliders and the floor colliders (unless we use edit distance or something simular 
            DisableRoomColliders(BoxColliders); 
        }

        public static void SetLayerRecursively(GameObject go, int layerNumber)
        {
            foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = layerNumber;
            }
        }

        private void OnMouseDown()
        {
            GameMainManager.Instance.EventManager.TriggerEvent(this, EventManager.EventName.ChangeRoomInFocus.ToString(), EventArgs.Empty);
        }

        public void ChildOnMouseDown()
        {
            OnMouseDown();
        }

        public void EnableRoomColliders(IEnumerable<BoxCollider> colliders)
        {
            colliders.ToList().ForEach(x => x.enabled = true);
        }
        
        public void DisableRoomColliders(IEnumerable<BoxCollider> colliders)
        {
            colliders.ToList().ForEach(x => x.enabled = false);
        }
    }
}