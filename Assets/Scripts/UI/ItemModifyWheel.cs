using UnityEngine;
using UnityEngine.UI;

public class ItemModifyWheel : MonoBehaviour {

	// Use this for initialization
	public GameObject ItemModifyWheelParent;
	public Button ConfirmButton;
	public Button CancelButton;
	public Button RotateButton;
	public Transform CurrentlyTracking { get; set; }
	
	private bool _turnOffAfterClickIsProcessed = false;

	public void EnableUI(Transform trackTransform, bool isPucharsed = true)
	{
		ItemModifyWheelParent.SetActive(true);
		CurrentlyTracking = trackTransform;
	}
        
	public void DisableUI()
	{
		_turnOffAfterClickIsProcessed = true;
		CurrentlyTracking = null;
	}
	
	void Update()
	{
		if (CurrentlyTracking != null)
		{
			ItemModifyWheelParent.transform.position = Camera.main.WorldToScreenPoint(CurrentlyTracking.position + Vector3.up);
		}
	}

	private void LateUpdate()
	{
		if(_turnOffAfterClickIsProcessed)
			ItemModifyWheelParent.SetActive(false);
		_turnOffAfterClickIsProcessed = false;
	}
}
