using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Managers.SceneManagers;
using UnityEngine;
using UnityEngine.UI;

public class EditModeButton : MonoBehaviour
{
	private bool editState;
	private Button button;
	private Text text;
	// Use this for initialization
	void Start ()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(EditButtonPressed);
		text = button.GetComponentInChildren<Text>();
	}
	
	
	public void EditButtonPressed()
	{
		editState = !editState;
		ModifyText(editState);
		if (editState)
		{
			GameMainManager.Instance.EventManager.TriggerEvent(this, EventManager.EventName.EditRoomBottonPressed.ToString(), new EditButtonEventArgs(editState));
		}
		else
		{
			GameMainManager.Instance.EventManager.TriggerEvent(this, EventManager.EventName.EditRoomBottonExit.ToString(), new EditButtonEventArgs(editState));
		}
	}

	private void ModifyText(bool editState)
	{
		text.text = editState ? "Exit Edit Mode" : "Edit Mode";
	}
}
