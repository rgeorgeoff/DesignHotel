using System;
using BuildControllers;
using Managers;
using Managers.SceneManagers;
using UnityEngine;

namespace CameraScripts
{
	public class RoomInFocusHelper : MonoBehaviour
	{

		public Transform roomFocusHelper;

		private Vector3 lastUpdatedRoomCamPos = Vector3.zero;
		private bool _ableToChangeRoomSelection;

		private void Start()
		{
			GameMainManager.Instance.EventManager.StartListening(EventManager.EventName.CameraMoved.ToString(), CameraMoved);
		}

		private void CameraMoved(object caller, EventArgs ea)
		{
			//if camera is at the same position, forget-about-it!
			if (lastUpdatedRoomCamPos == transform.position || _ableToChangeRoomSelection) return;

			RoomController rc = HotelController.GetClosestRoomTo(roomFocusHelper.position,
				((MainGameSceneManager) GameMainManager.Instance.CurrentSceneManager)
				.HotelController.RoomControllers);

			//if we are trying to focus the room that already is in focus, no need to send event
			if (rc == GameMainManager.Instance.RoomSelectionManager.CurrentlyInFocusRoomController) return;

			//otherwise send we are focusing a new room
			GameMainManager.Instance.EventManager.TriggerEvent(this, EventManager.EventName.ChangeRoomInFocus.ToString(),
				new RoomSelectedEventArgs(rc));

			lastUpdatedRoomCamPos = transform.position;
		}

		public void LockRoomSelection()
		{
			_ableToChangeRoomSelection = false;
		}

		public void UnlockRoomSelection()
		{
			_ableToChangeRoomSelection = true;
		}
	}
}

