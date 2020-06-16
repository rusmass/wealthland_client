using UnityEngine;
using System.Collections;

/// <summary>
///  选择角色界面，滑动手机屏幕，旋转角色模型
/// </summary>
public class RoleRotate : MonoBehaviour {

	public float speed = 5;
	
	bool flag;
	bool isScale;
		
	float olddis = 0;
	float newdis = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.touchCount==1)
		{   
	        if(Input.GetTouch(0).phase == TouchPhase.Moved)
//			if(Input.GetMouseButton(0))
				{				
				float x=Input.GetAxis("Mouse X");
					if(x>0){
						Vector3 angle = transform.localEulerAngles;
						angle.y -= speed;
						transform.localEulerAngles = angle;

						
					}
					if(x<0){
						Vector3 angle = transform.localEulerAngles;
						angle.y += speed;
						transform.localEulerAngles = angle;

			  		}
				}
			}
		
		}

}

