using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class RoomWallManager : MonoBehaviour
{
	public List<WallEdgeManager> walls = new List<WallEdgeManager>();
	public Transform roomCenterTransform;
	// Use this for initialization
	void Start () {
		GameMainManager.Instance.EventManager.StartListening(EventManager.EventName.CameraMoved.ToString(), CameraMovedHandler);
	}

	private void CameraMovedHandler(object caller, EventArgs eventArgs)
	{
		CameraMovedEventArgs cea = eventArgs as CameraMovedEventArgs;
		//compare each side of the room to the camera position
		float disFromRoomCenterXZ = Vector3.Distance(Vector3.Scale(roomCenterTransform.position, new Vector3(1,0,1)), Vector3.Scale(cea.NewPos, new Vector3(1,0,1)));
		foreach (WallEdgeManager wall in walls)
		{
			wall.HandleCameraMoveWalls(disFromRoomCenterXZ, cea.NewPos);
		}
		foreach (WallEdgeManager wall in walls)
		{
			wall.HandleCameraMoveWallConnectors();
		}
	}
}
