using System.Collections;
using System.Collections.Generic;
using BuildControllers;
using Managers;
using Managers.SceneManagers;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class AddRoomController:MonoBehaviour {

	void Start()
	{
		GetComponent<Button>().onClick.AddListener(AddRoomForVerticalStack);
	}
	
	public void AddRoomForVerticalStack()
	{
		MainGameSceneManager msm = GameMainManager.Instance.CurrentSceneManager as MainGameSceneManager;
		if (msm != null)
			msm.HotelController.AddRoomFromScratchAtPos(new Vector3Int(0, 11 * msm.HotelController.RoomControllers.Count - 1, 0));
	}
}
