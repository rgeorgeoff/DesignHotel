using System;
using System.ComponentModel;
using Models;

namespace BuildControllers
{
    public interface IPlacableEditor : IManager
    {
        Placeable CurrentlySelected { get; set; }
        void AddPlaceableFromStore(Action<bool> returnFunc, PlaceableDefaultInfo placeableDefaultInfo);
    }
    
}