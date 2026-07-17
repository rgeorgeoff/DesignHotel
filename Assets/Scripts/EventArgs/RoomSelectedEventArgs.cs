using System;
using BuildControllers;

public class RoomSelectedEventArgs : EventArgs
{
    public RoomSelectedEventArgs(RoomController rc)
    {
        RoomController = rc;
    }
    
    public RoomController RoomController { get;}
}