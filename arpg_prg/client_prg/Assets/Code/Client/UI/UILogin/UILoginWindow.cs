using Client;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UILoginWindow : UIWindow<UILoginWindow, UILoginController>
	{
		protected override void _Init (GameObject go)
		{
//			_button = go.transform.DeepFindEx ("button");
//			var xxx = go.GetComponentEx<RawImage> ("background");
//			EventTriggerListener.Get (_button.gameObject).onClick = OnClickBtn;
//			Console.WriteLine ("___________________Init");
//
//			Console.WriteLine ("!!!!!!!!!!!!!!!" + xxx.texture.name);
//
//			var sp = go.GetComponentEx<Image> ("button");
//			_imageDisplay = new UIImageDisplay (sp);
//			_imageDisplay.Load ("share/atlas/common/1.ab");
//
//			_rawImageDisplay = new UIRawImageDisplay (xxx);
//			_rawImageDisplay.Load ("share/texture/3.ab");

			_OnInitCenter (go);
			_OnInitBottom (go);

		}

//		private void OnClickBtn(GameObject go)
//		{
//			_controller.setVisible (false);
//			Console.WriteLine ("!!!!!!!!!!!!!!!!!!!!!!!");
//		}

        protected override void _OnShow()
        {
			_OnShowCenter ();
			_OnShowBottom ();
        }

        protected override void _OnHide ()
		{
			_OnHideCenter ();
//			_OnShowCenter ();
			_OnHideBottom();
		}

		protected override void _Dispose ()
		{
			
		}

//		private Transform _button;
//		private UIImageDisplay _imageDisplay;
//		private UIRawImageDisplay _rawImageDisplay;
	}
}

