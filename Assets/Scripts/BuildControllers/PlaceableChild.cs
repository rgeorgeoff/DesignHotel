using UnityEngine;
using UnityEngine.EventSystems;

//Used for collision detection!
namespace BuildControllers
{
    public class PlaceableChild : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Placeable _parentPlacable;

        private void Awake()
        {
            _parentPlacable = GetComponentInParent<Placeable>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _parentPlacable.ChildOnMouseDown();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _parentPlacable.ChildOnMouseDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _parentPlacable.ChildOnMouseDrag();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _parentPlacable.OnMouseUp();
        }
    }
}