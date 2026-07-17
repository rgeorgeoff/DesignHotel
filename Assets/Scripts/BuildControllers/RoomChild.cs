using UnityEngine;
using UnityEngine.EventSystems;

//Used for collision detection!
namespace BuildControllers
{
    public class RoomChild : MonoBehaviour, IPointerClickHandler 
    {
        private Room _parentRoom;

        private void Awake()
        {
            _parentRoom = GetComponentInParent<Room>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _parentRoom.ChildOnMouseDown();
        }
    }
}