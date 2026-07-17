using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Models;
using UnityEngine;

namespace BuildControllers
{
	public class Placeable : MonoBehaviour
	{
		public Transform RotatePivotPoint { get; private set; }
		public RoomController ParentRoom { get; private set; }
		public PlaceableData PlaceableData { get; set; }
		public Vector3Int GridPos { get; }
		public bool BeingMoved { get; set; }
		public bool IsPurchased { get; set; }
		public Action OnMouseDown;
		public Action OnMouseUp;
		public Action OnMouseDrag;
		public MeshRenderer MeshRenderer;
		public List<Collider> Colliders;
		
		private void Awake()
		{
			RotatePivotPoint = transform.GetChild(0);
			MeshRenderer = GetComponentsInChildren<MeshRenderer>()[0];
			Colliders = GetComponentsInChildren<Collider>().ToList();
		}

		public Vector3Int GetSizeOfObject() // TBD
		{
			return PlaceableData.Size;
		}
		
		//Dont make these inits one function (idk why, but it borked otherwise)
		internal void Init(RoomController parentRoom, PlaceableDefaultInfo placeableDefaultInfo, PlaceableData pd, bool isPurchased = true)
		{
			IsPurchased = isPurchased;
			ParentRoom = parentRoom;
			placeableDefaultInfo.DefaultPlaceableData.ResourceFileLocation = placeableDefaultInfo.AssetBundlePath; //--TBD (i think we should get rid of placeable data and just use placeableDefaultInfo) 
			//create a copy of the placeabledata so that its our own reference now.
			PlaceableData = new PlaceableData(pd);
			RotateToRotationInt(PlaceableData.Rotation, RotatePivotPoint.transform);
		}

		internal void InitWithReference(RoomController parentRoom, PlaceableDefaultInfo placeableDefaultInfo, PlaceableData pd, bool isPurchased = true)
		{
			IsPurchased = isPurchased;
			ParentRoom = parentRoom;
			placeableDefaultInfo.DefaultPlaceableData.ResourceFileLocation = placeableDefaultInfo.AssetBundlePath; //--TBD (i think we should get rid of placeable data and just use placeableDefaultInfo) 
			//create a copy of the placeabledata so that its our own reference now.
			PlaceableData = pd;
			RotateToRotationInt(PlaceableData.Rotation, RotatePivotPoint.transform);
		}

		public static void RotateToRotationInt(int rotationInt, Transform placeableRotationPointTransform)
		{
			ResetRotation(placeableRotationPointTransform);
			switch (rotationInt)
			{
				case 1:
					placeableRotationPointTransform.Rotate(Vector3.forward, 90);
					break;
				case 2:
					placeableRotationPointTransform.Rotate(Vector3.forward, 180);
					break;
				case 3:
					placeableRotationPointTransform.Rotate(Vector3.forward, 270);
					break;
			}
		}

		public static void ResetRotation(Transform placeableRotationPointTransform)
		{
			placeableRotationPointTransform.localRotation = Quaternion.identity;
		}

		internal void ChildOnMouseDown()
		{
			GameMainManager.Instance.EventManager.TriggerEvent(this,
				EventManager.EventName.PlacableSelected.ToString(), new PlacableSelectedEventArgs(this));
			if(OnMouseDown != null)
				OnMouseDown.Invoke();
		}

		internal void ChildOnMouseUp()
		{
			if(OnMouseUp != null)
				OnMouseUp.Invoke();
		}

		internal void ChildOnMouseDrag()
		{
			if(OnMouseDrag != null)
				OnMouseDrag.Invoke();
		}

		//asks the room model if the position we are hovering over when moving an item is a valid on or not
		//if it is, we highlight it green, else red.
		internal void ValidPlacmentViewUpdate()
		{
		}

		//repositions placable based on what the roommodel data says.
		internal void ReinitializeViewFromController()
		{
		}

		internal void RotateLeft()
		{
			RotatePivotPoint.transform.Rotate(Vector3.forward, 90);
			PlaceableData.Rotation += 1;
			if (PlaceableData.Rotation > 3)
			{
				PlaceableData.Rotation = 0;
			}
		}

		internal void RotateRight()
		{
			RotatePivotPoint.transform.Rotate(Vector3.forward, -90);
			PlaceableData.Rotation -= 1;
			if (PlaceableData.Rotation < 0)
			{
				PlaceableData.Rotation = 3;
			}
		}

		public void DisableColliders()
		{
			if (Colliders.Count > 0)
			{
				foreach (Collider collider1 in Colliders)
				{
					collider1.enabled = false;
				}
			}
		}
		
		public void EnableColliders()
		{
			foreach (Collider collider1 in Colliders)
			{
				collider1.enabled = true;
			}
		}

		public static List<Vector3Int> SpotsTakenOnTheGridBasedOffPos(PlaceableData pd, Vector3Int pos)
		{
			List<Vector3Int> spotsToCheck = new List<Vector3Int>();
			for (int x = 0; x < pd.Size.x; x++)
			{
				for (int z = 0; z < pd.Size.z; z++)
				{
					if (pd.Rotation == 0 || pd.Rotation == 2)
					{
						spotsToCheck.Add(new Vector3Int(pos.x - pd.Size.x/2 + x, pos.y, pos.z - pd.Size.z/2 + z));
					}
					else
					{
						spotsToCheck.Add(new Vector3Int(pos.x - pd.Size.z/2 + z, pos.y, pos.z - pd.Size.x/2 + x));
					}
				}
			}
			return spotsToCheck;
		}
		
		//Debug & Dev
		public Ray _mouseRay;
		public Ray _castRay;
		private Vector3 gizSphere;
		private bool _rayEnabled = true;

		private void OnDrawGizmosSelected()
		{
			if (!_rayEnabled) return;
			// Draws a 5 unit long red line in front of the object
			Gizmos.color = Color.red;
			Gizmos.DrawLine(_mouseRay.origin, _mouseRay.direction * 100  + _mouseRay.origin);
			Gizmos.color = Color.green;
			Gizmos.DrawRay(_castRay.origin, _castRay.direction * 100  + _castRay.origin);
			Gizmos.DrawSphere(gizSphere, .2f);
		}
	}
}