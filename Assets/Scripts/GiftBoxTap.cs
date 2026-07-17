using UnityEngine;
using UnityEngine.EventSystems;

public class GiftBoxTap : MonoBehaviour, IPointerClickHandler
{

	public void OnPointerClick(PointerEventData eventData)
	{
		SendMessageUpwards("TapBox");
	}
}

