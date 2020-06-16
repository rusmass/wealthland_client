using System;
using UnityEngine;
using UnityEngine.UI;
namespace Client.UI
{
	public class ChangeCloseItem
	{
		public ChangeCloseItem (GameObject go,string OpenPath,string ClosePath)
		{
			_OnInitItem (go);
			_openPath = OpenPath;
			_closePath = ClosePath;
		}

		private void _OnInitItem(GameObject go)
		{
			var img = go.GetComponent<Image> ();
			_bgImg = new UIImageDisplay (img);
			btn_roll = go.GetComponentEx<Button> ("itembtn");
			_imgWidth = go.GetComponent<RectTransform> ().sizeDelta.x;

			_initPostion = btn_roll.transform.localPosition;
		}

		private bool IsOpened
		{
			set
			{
				_isOpened = value;
				if (_isOpened == true)
				{
					_setOpen ();
				}
				else
				{
					_setClose ();
				}
			}

			get
			{ 
				return _isOpened;
			}
			
		}

		private void _setOpen()
		{
			if (null != _bgImg)
			{
				_bgImg.Load (_openPath);
			}

			btn_roll.transform.localPosition = new Vector3 (-_imgWidth/2,_initPostion.y,_initPostion.z);
		}

		private void _setClose()
		{
			if (null != _bgImg)
			{
				_bgImg.Load (_closePath);
			}

			btn_roll.transform.localPosition=new Vector3(_imgWidth/2,_initPostion.y,_initPostion.z);
		}


		public void _OnDisposeItem()
		{
			if (null != _bgImg)
			{
				_bgImg.Dispose();
			}
		}

		public Button GetButton
		{
			get
			{
				return btn_roll;
			}
		}

		private UIImageDisplay _bgImg;
		private Button btn_roll;

		private string _openPath;
		private string _closePath;

		private float _imgWidth=0;

		private bool _isOpened = false;

		private Vector3 _initPostion;


	}
}

