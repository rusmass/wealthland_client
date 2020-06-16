using System;

namespace Client.UI
{
	public class UIGameSimpleTipController:UIController<UIGameSimpleTipWindow,UIGameSimpleTipController>
	{
		public UIGameSimpleTipController ()
		{
		}

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/gametipsimple.ab";
			}
		}

		/// <summary>
		/// Sures the sure tip. 
		/// </summary>
		/// <param name="_sureAction">Sure action.</param>
		public void SetSureTip(string _tipStr , Action _sureAction=null)
		{
			callSure = _sureAction;
			_type = 1;
			txtStr = _tipStr;
		}

		/// <summary>
		/// Sets the cancle tip.
		/// </summary>
		/// <param name="_calcleAction">Calcle action.</param>
		public void SetCancleTip(string _tipStr ,Action _calcleAction=null)
		{
			callCancle = _calcleAction;
			_type = 0;
			txtStr = _tipStr;
		}

		public string txtStr="";

		/// <summary>
		/// The call sure.  点击确定的回调函数
		/// </summary>
		public Action callSure=null;

		/// <summary>
		/// The call cancle. 点击取消的回调函数
		/// </summary>
		public Action callCancle=null;

		/// <summary>
		/// The type.  
		/// </summary>
		private int _type=0;

		/// <summary>
		/// Gets the type of the tip. 窗口的类型，0显示取消按钮  ，1显示确定按钮
		/// </summary>
		/// <value>The type of the tip.</value>
		public int TipType
		{
			get
			{
				return _type;
			}
		}
	}
}

