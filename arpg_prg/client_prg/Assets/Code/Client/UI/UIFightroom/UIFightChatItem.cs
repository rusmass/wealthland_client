using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class UIFightChatItem
	{
		public UIFightChatItem (GameObject go)
		{
//			lb_name = go.GetComponentEx<Text> (Layout.lb_name);
			lb_name = go.GetComponent<Text>();
			lb_txt = go.GetComponentEx<Text> (Layout.lb_txt);

			if (isUpdated == false)
			{
				txtNamePosition = lb_name.rectTransform.localPosition;
				txtChatPosition = lb_txt.rectTransform.localPosition;
				isUpdated = true;
			}


		}

		public void Refresh(NetChatVo value)
		{
//			Console.Error.WriteLine ("sssssssssssssssss"+value.playerName);
			lb_name.text = value.playerName+":";
			lb_txt.text = value.chat;
			lb_txt.rectTransform.localPosition =new Vector3( txtNamePosition.x+lb_name.preferredWidth+10,txtChatPosition.y,txtChatPosition.z);
			_chatvo = value;
		}

		/// <summary>
		/// Disposes the self.释放资源
		/// </summary>
		public void DisposeSelf()
		{
			
		}

		private NetChatVo _chatvo;
		private Text lb_name;
		private Text lb_txt;

		private static Vector3 txtNamePosition;
		private static Vector3 txtChatPosition;
		private static bool isUpdated=false;

		class Layout
		{			
			public static string lb_name="txtname";
			public static string lb_txt="txtmessage";
		}
	}
}

