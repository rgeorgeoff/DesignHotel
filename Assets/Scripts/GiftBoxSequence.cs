using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftBoxSequence : MonoBehaviour
{

	[SerializeField] private GameObject truck;
	[SerializeField] private GameObject giftBox;
	[SerializeField] private GameObject popUp;
	[SerializeField] private GameObject shine;
	[SerializeField] private GameObject confetti;

	private Animator truckAnim;
	private Animator giftBoxAnim;
	private Animator popUpAnim;


	void Start ()
	{
		truckAnim = truck.GetComponentInChildren<Animator>();
		giftBoxAnim = giftBox.GetComponentInChildren<Animator>();
		popUpAnim = popUp.GetComponent<Animator>();

	}
	
	public void GiftSequenceGo()
	{
		truck.SetActive(true);
		truckAnim.SetTrigger("GiftSeqStart");
		
	}

	private void GiftPopUpSequenceGo()
	{
		popUp.SetActive(true);
		giftBoxAnim.SetTrigger("BoxJump");
		popUpAnim.SetTrigger("PopUpStart");
	}

	public void TapBox()
	{
		giftBoxAnim.SetTrigger("BoxOpen");
		confetti.SetActive(true);
		shine.SetActive(true);
	}

	private void Update()
	{
		if (shine.activeInHierarchy == true)
		{
			shine.GetComponent<Transform>().transform.Rotate(0, 0, 60 * Time.deltaTime);
		}
	}
}
