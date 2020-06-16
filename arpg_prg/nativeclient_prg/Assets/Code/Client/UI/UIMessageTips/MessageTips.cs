using Client.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Client
{
    public class MessageTips
    {
		private static string _tmpStr="";		


		public static void Show(string text, TweenCallback callBack = null,bool hideWord=false)
        {

            //return;

			_tmpStr=text;

			var control = UIControllerManager.Instance.GetController<UIMessageTipsController>();
			if (null != control)
			{
				if (!control.getVisible())
				{
					control.setVisible(true);
				}

				// 如果是不隐藏窗口   ,, 不管关闭不关闭都要显示
				if (hideWord == true)
				{
					if (null != callBack)
					{
						control.ShowFlyText("", callBack);
					}
				}
				else
				{
					control.ShowFlyText(text, callBack);
				}


                Audio.AudioManager.Instance.Tip_ReturnedMe(PlayerManager.Instance.HostPlayerInfo.careerID);

            }
        }

		/// <summary>
		/// Releases all resource used by the <see cref="Client.MessageHint"/> object.  移除弹字数据数据
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Client.MessageHint"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Client.MessageHint"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="Client.MessageHint"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Client.MessageHint"/> was occupying.</remarks>
		public static void Dispose()
		{
			var control = UIControllerManager.Instance.GetController<UIMessageTipsController>();
			control.Dispose ();
		}

    }
}
