using System;
using System.Collections.Generic;
using System.Linq;
using BuildControllers;
using Managers.SceneManagers;

namespace Managers
{
    public class RoomSelectionManager
    {
        public RoomController CurrentlySelectedRoomController;
        public RoomController CurrentlyInFocusRoomController;
        
        //This guy keeps track of what room is in focus and if the player zooms in far enough onto a room it becomes editable?

        public RoomSelectionManager()
        {
            GameMainManager.Instance.EventManager.StartListening(EventManager.EventName.ChangeRoomInFocus.ToString(),
                NewRoomInFocusEvent);
            GameMainManager.Instance.EventManager.StartListening(EventManager.EventName.EditRoomBottonPressed.ToString(),
                RoomSelectedForEdit);
            GameMainManager.Instance.EventManager.StartListening(EventManager.EventName.EditRoomBottonExit.ToString(),
                RoomDeselectedForEdit);
        }

        public void NewRoomInFocusEvent(object sender, EventArgs ep)
        {
            RoomSelectedEventArgs rsea = ep as RoomSelectedEventArgs;

            if (rsea != null && rsea.RoomController != CurrentlySelectedRoomController)
            {
                if(CurrentlySelectedRoomController != null)
                    DeselectRoom(CurrentlySelectedRoomController);
                CurrentlyInFocusRoomController = rsea.RoomController;
            }
        }

        public void RoomSelectedForEdit(object sender, EventArgs ep)
        {
            if (CurrentlyInFocusRoomController != null)
            {
                CurrentlySelectedRoomController = CurrentlyInFocusRoomController;
                RoomController.Selected(CurrentlySelectedRoomController);
            }
        }
        
        public void RoomDeselectedForEdit(object sender, EventArgs ep)
        {
            if (CurrentlyInFocusRoomController != null)
            {
                RoomController.Deselected(CurrentlySelectedRoomController);
                CurrentlySelectedRoomController = null;
            }
        }
        
        public void RoomDeselectedEvent(object sender, EventArgs ep)
        {
            RoomSelectedEventArgs rsea = ep as RoomSelectedEventArgs;
            //if (rsea != null) _currentlySelectedRooms.Remove(rsea.RoomController);
            if (rsea != null && CurrentlySelectedRoomController == rsea.RoomController)
            {
                DeselectRoom(CurrentlySelectedRoomController);
                CurrentlySelectedRoomController = null;
            }
        }

        private void DeselectRoom(RoomController rc)
        {
            if (rc != null)
            {
                RoomController.Deselected(rc);
            }
        }
    }
}