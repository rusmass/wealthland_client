using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIBalanceFixedInforWindow
	{
		private void _InitBottom (GameObject go)
		{
			_btnSure = go.GetComponentEx<Button> (Layout.btn_sure);
			_btnCancle = go.GetComponentEx<Button> (Layout.btn_cancle);
			_btnCancle.SetActiveEx (false);
//			_SetSureBtnContent (_btnSure);
		}


		private void _SetSureBtnContent(Button _button)
		{
			var img = _button.gameObject.GetComponentEx<Image> ("Image");
			_imgLoad = new UIImageDisplay (img);
			_imgLoad.Load (UIOtherCardWindowController.imgSurePath);
		}


		private void _OnShowBottom()
		{
//			var vec3 = _btnSure.transform.localPosition;
//			_btnSure.transform.localPosition = new Vector3 (_positionBtnSureX,vec3.y,vec3.z);			
			EventTriggerListener.Get (_btnSure.gameObject).onClick += _onSureHandler;	
		}

		private void _OnHideBottom()
		{
			EventTriggerListener.Get (_btnSure.gameObject).onClick -= _onSureHandler;
		}


		private void _OnDisposeBottom()
		{
			if(null != _imgLoad)
			{
				_imgLoad.Dispose ();
			}
		}


		private void _onSureHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_controller.setVisible(false);
		}

		private Button _btnSure;
		private Button _btnCancle;

		private Counter _timer;

		//private float _positionBtnSureX=0;

		private UIImageDisplay _imgLoad;
	}
}

