using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraScale : MonoBehaviour 
{
	void Start () 
	{
		int ManualWidth = (int)(Core.Constants.UiResolution.x);
		int ManualHeight = (int)(Core.Constants.UiResolution.y);
		int manualHeight;
		if (System.Convert.ToSingle (Screen.height) / Screen.width > System.Convert.ToSingle (ManualHeight) / ManualWidth) 
		{
			manualHeight = Mathf.RoundToInt (System.Convert.ToSingle (ManualWidth) / Screen.width * Screen.height);
		}
		else 
		{
			manualHeight = ManualHeight;
		}
		Camera camera = GetComponent<Camera>();
		float scale =System.Convert.ToSingle(manualHeight / ManualHeight);
		camera.fieldOfView*= scale;
	}
}