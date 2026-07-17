using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEdgeManager : MonoBehaviour
{
	public GameObject wallConnectionLeft;
	public GameObject wallConnectionRight;
	private GameObject leftWallConnector;
	private GameObject rightWallConnector;
	private float buffer = -0.9f;

	private void Start()
	{
		leftWallConnector = transform.Find(name + "EdgeLeft").gameObject;
		rightWallConnector = transform.Find(name + "EdgeRight").gameObject;
	}

	public void HandleCameraMoveWalls(float disFromRoomCenter, Vector3 ceaNewPos)
	{
		Vector3 ceaNewPosXZ = Vector3.Scale(ceaNewPos, new Vector3(1, 0, 1));
		Vector3 myPosXZ = Vector3.Scale(transform.position, new Vector3(1, 0, 1));
		gameObject.SetActive(disFromRoomCenter + buffer < Vector3.Distance(myPosXZ, ceaNewPosXZ));
	}
	
	public void HandleCameraMoveWallConnectors()
	{
		leftWallConnector.SetActive(!wallConnectionLeft.activeSelf);
		rightWallConnector.SetActive(!wallConnectionRight.activeSelf);
	}
}
