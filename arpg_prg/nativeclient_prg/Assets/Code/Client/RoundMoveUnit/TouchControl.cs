#if UNITY_ANDROID && !UNITY_EDITOR
#define ANDROID
#endif


#if UNITY_IPHONE && !UNITY_EDITOR
#define IPHONE
#endif

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using Client.Cameras;
using Client.UI;
using Client;

/// <summary>
/// 触摸ui
/// </summary>
public class TouchControl : MonoBehaviour 
{

		private float x = 0.0f;
		private float y = 0.0f;

		private float xSpeed = 10.0f;
		private float ySpeed = 5.0f;

		bool m_bool_ui;

		// Use this for initialization
		void Start () 
		{
			m_bool_ui = false;
		}
		
//		// Update is called once per frame
		void Update () 
		{
			if (Input.GetMouseButtonDown(1)||(Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Began))
			{
				#if IPHONE || ANDROID
				if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
				#else
				if (EventSystem.current.IsPointerOverGameObject())
				#endif
				{
					Debug.Log("当前触摸在UI上");
					m_bool_ui = true;
				}
				else 
				{
					Debug.Log("当前没有触摸在UI上");
					m_bool_ui = false;
				}
			}
//		}
		if(m_bool_ui == false)
		{
			if(Input.touchCount == 1)
//			if(Input.GetMouseButton(1))
			{
				if(Input.GetTouch(0).phase == TouchPhase.Moved)
				{

					x -= Input.GetAxis("Mouse X") * xSpeed * 0.005f;
					y -= Input.GetAxis("Mouse Y") * ySpeed * 0.01f;

					if(x >= 1.5f)
					{
						x = 1.5f;
					}

					if(x <= -1.5f)
					{
						x = -1.5f;
					}

					if(y >= 2.0f)
					{
						y = 2.0f;
					}

					if(y <= -2.0f)
					{
						y = -2.0f;
					}

					Vector3 position =  new Vector3(x,y, 0) ;
					gameObject.transform.localPosition = position;
				}

				if(Input.GetTouch(0).phase == TouchPhase.Ended)
				{
					SmartCamera.Instance.SetCameraPos();
					x = y = 0;
				}
			}

//			if(Input.GetMouseButtonUp(1))
//			{
//				SmartCamera.Instance.SetCameraPos();
//				x = y = 0;
//			}
		}

	}
}

