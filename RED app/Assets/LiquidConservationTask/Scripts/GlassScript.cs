using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassScript : MonoBehaviour {
    [Header ("Glass Properties")]

    public float Width;
    public float Height;
    public float Area;
    public float SetArea;  //Sets the area of WATER filled!
    public float SetLiquidHeight;
    public float FillRate;
    public bool SetMarkerBool = false;
    public float SetMarkerAreaPosition;
	public float CurrentWaterArea; 
	public float CurrentWaterHeight; //Nb. this has a max of the glass height - cannot be overpoured!!
	public bool AllowFill; //bool to prevent filling after first click
	public float GlassWidth;
	private bool stopUnpour;

    [Header("Game Objects")]
	public GameObject SceneManagerGO;
    public GameObject Water;
    public GameObject MarkerGO;


	// Use this for initialization
	void Awake () {
		//AllowFill = true;
		SceneManagerGO = GameObject.Find ("SceneManagerGO");
		FillRate = SceneManagerGO.GetComponent<LC_SceneManagerScript> ().GlassFillRate;
        Water = gameObject.transform.Find("WaterMask").gameObject.transform.Find("Water").gameObject;
        MarkerGO = gameObject.transform.Find("MarkerLine").gameObject;
		//stopUnpour = false;

		SetGlassArea (300*400); //this is overwritten in scenemanager script!

        //StartCoroutine("Pour", 10000f);
    }

    void Update()
    {

        if (Water.transform.localPosition.y < -Height) {
            StopPour();
            Water.transform.localPosition = new Vector3(0, -Height, 0);
        }
        if (Water.transform.localPosition.y > 0)
        {
            StopPour();
            Water.transform.localPosition = new Vector3(0, 0, 0);
        }

		CurrentWaterHeight = Height + Water.transform.localPosition.y; //Current height of the water in glass
		CurrentWaterArea = CurrentWaterHeight * Width;

    }

	public void SetGlassArea(float areaToSet) //Input which will set the rect scale of the parent object. 
	{
		
		float currentWidth = Water.GetComponent<RectTransform>().rect.width;
		float currentHeight = Water.GetComponent<RectTransform>().rect.height;
		float scaleXBy = areaToSet / (currentWidth * currentHeight);

		gameObject.transform.GetComponent<RectTransform> ().localScale = new Vector2 (scaleXBy, 1);

		Height = (float)currentHeight;
		Width = currentWidth*scaleXBy;
		GlassWidth = gameObject.transform.Find ("Glass").gameObject.GetComponent<RectTransform> ().rect.width * scaleXBy;
		//print ("Glass W " + GlassWidth.ToString ());
		//print ("Scale X " + scaleXBy.ToString ());


		Area = Width * Height;
		if (SetArea > Area) { SetArea = Area; }
		if (SetMarkerAreaPosition > Area) { SetMarkerAreaPosition = Area; }

		SetMarker();

		SetFill();



	}


    public void SetFill()
    {
        SetLiquidHeight = SetArea / Width;
        Water.transform.localPosition =new Vector3(0, SetLiquidHeight-Height,0);
    }

	public void SetWaterFillToArea(float waterArea) //fill the glass to specific area (similar to above but takes inputs!)
	{
		SetLiquidHeight = waterArea / Width;
		Water.transform.localPosition =new Vector3(0, SetLiquidHeight-Height,0);
	}




    public void StopPour()
    {
        //print("StopPour Run");
        Water.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
		stopUnpour=true;
    }

    public IEnumerator Pour(float areaFill)
    {
		//if (AllowFill) 
		//{
			//print ("Pour Run");
			FillRate = SceneManagerGO.GetComponent<LC_SceneManagerScript> ().GlassFillRate;

			Water.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, FillRate / Width, 0);
			yield return new WaitUntil (() => (Water.GetComponent<Rigidbody2D> ().velocity.y == 0) | Water.transform.localPosition.y >= (areaFill / Width) - Height);
			Water.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, 0, 0);
			
		//}
    }

    public IEnumerator UnPour(float areaFill)
    {
		stopUnpour = false;
        //print("UnPour Run");
		FillRate = SceneManagerGO.GetComponent<LC_SceneManagerScript> ().GlassFillRate;

        Water.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -FillRate/Width, 0);
		yield return new WaitUntil(() => (Water.transform.localPosition.y <= (areaFill / Width) - Height) | stopUnpour==true);
        Water.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
		//print ("Unpour finished");
    }

    public void SetMarker()
    {
        if (SetMarkerBool==false) { MarkerGO.SetActive(false); }

        MarkerGO.transform.localPosition = new Vector3(0, SetMarkerAreaPosition / Width - Height/2, 0);

    }
	public void SetMarkerFunction(float AreaInput)
	{
		MarkerGO.SetActive (true);
		MarkerGO.transform.localPosition = new Vector3(0, AreaInput / Width - Height/2, 0);

	}


}
