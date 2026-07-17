using System;
using System.Collections.Generic;

namespace Managers
{
    public class EventManager
    {
        private Dictionary<string, EventHandler<EventArgs>> _eventDictionaryWithParam;

        //I think its best to register all the events we want to use here, but strings are ok
        public enum EventName
        {
            //NEW_SCENE, // whenever the game changes scenes // Use the Scene Manager Events for this
            MainGameSceneLoaded, //specific scene finished loading
            DisableCameraMovement,
            EnableCameraMovement,
            PlacableSelected,
            ChangeRoomInFocus,
            RoomSelectedForEditing,
            RoomDeselected,
            CameraMoved,
            EditRoomBottonPressed,
            EditRoomBottonExit,
            EnteredEditMode,
            ExitEditMode
        }

        public EventManager()
        {
            Init();
        }

        public void Init()
        {
            _eventDictionaryWithParam = new Dictionary<string, EventHandler<EventArgs>>();
        }

        public void StartListening(string eventName, EventHandler<EventArgs> listener)
        {
            EventHandler<EventArgs> thisEvent;
            if (_eventDictionaryWithParam.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += listener;
                _eventDictionaryWithParam[eventName] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                _eventDictionaryWithParam.Add(eventName, thisEvent);
            }
        }

        public void StopListening(string eventName, EventHandler<EventArgs> listener)
        {
            EventHandler<EventArgs> thisEvent;
            if (!_eventDictionaryWithParam.TryGetValue(eventName, out thisEvent)) return;
            thisEvent -= listener;
            _eventDictionaryWithParam[eventName] = thisEvent;
        }

        public void TriggerEvent(object eventSender, String eventName, EventArgs ep)
        {
            EventHandler<EventArgs> thisEvent = null;
            if (_eventDictionaryWithParam.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(eventSender, ep);
            }
        }
    }
}