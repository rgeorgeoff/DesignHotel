using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckAnimationEvents : MonoBehaviour {

	public void TruckSpinFinished(float num)
	{
		if (num == 1)
		{
			SendMessageUpwards("GiftPopUpSequenceGo");
		}
	}
}
