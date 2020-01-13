using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTracker : MonoBehaviour {

	public bool isLeftChoice = false;
	public bool isRightChoice = false;

	public void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log (collision); 
		if (collision.gameObject.name == "left")
			isLeftChoice = true;
		else if (collision.gameObject.name == "right")
			isRightChoice = true;

	}

	//public void OnCollisionExit2D(Collision2D collision)
	//{
	//	if (collision.gameObject.name == "left")
	//		isLeftChoice = false;
	//	else if (collision.gameObject.name == "right")
	//		isRightChoice = false;
	//}

}
