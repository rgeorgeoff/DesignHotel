using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatFinger : MonoBehaviour
{
	public Canvas Canvas;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Input.mousePosition;
		//follow the mouse
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas.transform as RectTransform, Input.mousePosition, Canvas.worldCamera, out pos);
		transform.position = Canvas.transform.TransformPoint(pos);
	}
}
