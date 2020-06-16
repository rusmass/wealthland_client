using Client.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Client
{
    public class MessageHint
    {
		private static string _tmpStr="";

		private static int _sameCount=0;


		public static void Show(string text, TweenCallback callBack = null,bool hideWord=false)
        {
			var battleController = UIControllerManager.Instance.GetController<UIBattleController> ();		

			if(null != battleController)
			{
				battleController.SetTipRecord (text);
			}

			if (_tmpStr ==text )
			{
				MessageHint._sameCount+=1;
				if (MessageHint._sameCount >= 2)
				{
					return;
				}
			}
			else
			{
				MessageHint._sameCount = 0;
			}

			_tmpStr=text;

			var control = UIControllerManager.Instance.GetController<UIMessageHintController>();
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

//				if (UIBattleController.isOpenRecordWindow == false)
//				{
//					if (hideWord == true)
//					{
//						control.ShowFlyText(text, callBack);						
//					}
//					else
//					{
//						
//					}
//				}  
//				else
//				{
//					if (hideWord == true)
//					{
//						control.ShowFlyText(text, callBack);
//					}
//				}
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
			var control = UIControllerManager.Instance.GetController<UIMessageHintController>();
			control.Dispose ();
		}

    }
}
