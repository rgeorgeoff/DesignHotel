using System;
using BuildControllers;

public class PlacableSelectedEventArgs : EventArgs
{
    public PlacableSelectedEventArgs(Placeable p)
    {
        Placable = p;
    }
    public Placeable Placable { get; set; }
}