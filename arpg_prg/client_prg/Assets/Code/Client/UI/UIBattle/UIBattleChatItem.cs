using System;
using UnityEngine;
using UnityEngine.UI;
namespace Client.UI
{
	public class UIBattleChatItem
	{
		public UIBattleChatItem (GameObject go)
		{
			lb_name = go.GetComponent<Text> ();
			lb_txt = go.GetComponentEx<Text> ("txtchat");

			if (isUpdated == false)
			{
				txtNamePostion = lb_name.rectTransform.localPosition;
				txtChatPostion = lb_txt.rectTransform.localPosition;
				isUpdated = true;
			}
		}


		public void Refresh(NetChatVo value)
		{
			var tmpName = value.playerName;
			if (tmpName.Length > 6)
			{
				tmpName = tmpName.Substring (0, 6);
			}
			
			lb_name.text = tmpName+":";
			lb_txt.text = value.chat;

			lb_txt.rectTransform.localPosition = new Vector3 (txtNamePostion.x+lb_name.preferredWidth+10,txtChatPostion.y,txtChatPostion.z);

			_chatvo = value;
		}

		private static Vector3 txtNamePostion;
		private static Vector3 txtChatPostion;
		private static bool isUpdated=false;

		private NetChatVo _chatvo;
		private Text lb_name;
		private Text lb_txt;
	}
}

