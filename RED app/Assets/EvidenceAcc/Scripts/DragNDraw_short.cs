using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine;
using UnityEngine.EventSystems;

public class DragNDraw_short : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static GameObject DraggedInstance;

	Vector3 _startPosition;
	Vector3 _offsetToMouse;
	float _zDistanceToCamera;

	//RDK script
	RDK_short rdk_short; 
	//rdkscript object
	GameObject rdk_object;

	#region Interface Implementations

	//stuff for drawing line 
	private LineRenderer line;
	private Vector2 mousePosition;
	[SerializeField] private bool simplifyLine = false;
	[SerializeField] private float simplifyTolerance = 0.02f;

	private void Start () {
		line = GetComponent<LineRenderer>();
		rdk_object = GameObject.Find("GameObject"); 

	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		//check if we are allowed to drag by getting main RDK script only drag if we are allowed to 
		rdk_short = rdk_object.GetComponent<RDK_short>(); 
		if (rdk_short.DraginNDrawin == true) {
			//let the RDK script know we are moving !
			rdk_short.draggin = true; 

			DraggedInstance = gameObject;
			_startPosition = transform.position;
			_zDistanceToCamera = Mathf.Abs (_startPosition.z - Camera.main.transform.position.z);

			_offsetToMouse = _startPosition - Camera.main.ScreenToWorldPoint (
				new Vector3 (Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)
			);
		}
	}

	public void OnDrag (PointerEventData eventData)
	{
		//check if we are allowed to drag by getting main RDK script only drag if we are allowed to 
		rdk_short = rdk_object.GetComponent<RDK_short>(); 
		if (rdk_short.DraginNDrawin == true) {
			if (Input.touchCount > 1)
				return;

			mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			line.positionCount++;
			line.SetPosition (line.positionCount - 1, mousePosition);

			transform.position = Camera.main.ScreenToWorldPoint (
				new Vector3 (Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)
			) + _offsetToMouse;

			//clamp to screen coordiantes
			transform.position = new Vector3 (Mathf.Clamp (transform.position.x, -6f, 6f), Mathf.Clamp (transform.position.y, -5f, 3.5f), transform.position.z);

		} else {
			return;
		}
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		Debug.Log("end drag"); 
	rdk_short = rdk_object.GetComponent<RDK_short>(); 
		if (rdk_short.DraginNDrawin == true) {
			DraggedInstance = null;
			_offsetToMouse = Vector3.zero;
		}
	}

	#endregion
}