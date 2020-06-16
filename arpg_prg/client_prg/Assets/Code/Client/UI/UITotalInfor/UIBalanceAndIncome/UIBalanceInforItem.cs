using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
	public class UIBalanceInforItem:IDisposable
	{
		public UIBalanceInforItem (GameObject go)
		{
			lb_title = go.GetComponentEx<Text> (Layout.title);
			lb_num = go.GetComponentEx<Text> (Layout.num);
		    
			var tmpImg = go.GetComponentEx<Image> (Layout.img_pic);
			img_pic = new UIImageDisplay (tmpImg);

			btn_showInfor = go.GetComponentEx<Button> (Layout.img_pic);

			EventTriggerListener.Get(btn_showInfor.gameObject).onClick+=_OnClieckHandler;

		}

		public void Refresh(ChanceFixed value)
		{

			lb_title.text = value.title;
			lb_num.text = value.baseNumber.ToString ();
			if (null != img_pic)
			{
				img_pic.Load (value.cardPath);
			}

			_selfVo = value;
		}

		private void _OnClieckHandler(GameObject go)
		{
			Console.WriteLine ("sssssssssssssssssssssssssssssssssssssszzzzzzzzzzzzzzzz");
			Audio.AudioManager.Instance.BtnMusic ();
			var _controller = UIControllerManager.Instance.GetController<UIBalanceAndIncomeWindowController> ();
			var chanceList = _controller.GetShareList ();

			var isShare = false;

			for (var i = 0; i < chanceList.Count; i++)
			{
				var tmpShare = chanceList [i];
				if (tmpShare.id == _selfVo.id)
				{
					isShare = true;
					Console.WriteLine ("显示股票卡牌信息");

					var controller = UIControllerManager.Instance.GetController<UIBalanceShareInforWindowController> ();
					controller.cardData = tmpShare;
					controller.setVisible (true);
					break;
				}
			}

			if (isShare == false)
			{
				Console.WriteLine ("显示固定资产的信息");
				var controller = UIControllerManager.Instance.GetController<UIBalanceFixedInforWindowController> ();
				controller.cardData = _selfVo;
				controller.setVisible (true);

			}

		}

		public void Dispose()
		{
			if (null != img_pic)
			{
				img_pic.Dispose ();
			}			
		}

		private Text lb_title;
		private Text lb_num;
		private UIImageDisplay img_pic;
		private Button btn_showInfor;

		private ChanceFixed _selfVo;


		class Layout
		{
			public static string title = "lb_title";
			public static string num = "num";
			public static string img_pic = "img_pic";			
		}
	}
}

