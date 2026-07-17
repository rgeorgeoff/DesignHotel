using System;
using UnityEngine;

public class CameraMovedEventArgs : EventArgs
{
    public CameraMovedEventArgs(Vector3 newPos)
    {
        NewPos = newPos;
    }

    public Vector3 NewPos { get; set; }
}