using System;

public class EditButtonEventArgs : EventArgs
{
	public EditButtonEventArgs(bool editState)
	{
		this.editState = editState;
	}
	
	public bool editState { get; }
}