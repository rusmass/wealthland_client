using System;

namespace Client.UI
{
    /// <summary>
    /// 未引用
    /// </summary>
	public class UIConditionWindowController:UIController<UIConditionWindow,UIConditionWindowController>
	{
		public UIConditionWindowController ()
		{
		}


		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uicondition.ab";
			}
		}

		protected override void _OnLoad ()
		{

		}

		protected override void _OnShow ()
		{

		}

		protected override void _OnHide ()
		{

		}

		protected override void _Dispose ()
		{

		}

		/// <summary>
		/// 显示游戏条件提示 0 提示进入内圈的条件，1提示进入游戏成功的条件
		/// </summary>
		public int showConditionType=0;
	}
}

