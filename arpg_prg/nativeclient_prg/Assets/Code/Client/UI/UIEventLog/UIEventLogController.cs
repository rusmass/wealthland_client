using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Client.UI
{
    /// <summary>
    /// 显示遇到的卡牌事件
    /// </summary>
	public class UIEventLogController : UIController<UIEventLogWindow,UIEventLogController>
	{
		protected override string _windowResource
		{
			get{
				return "prefabs/ui/scene/uieventlog.ab";
			}
		}

		protected override void _OnLoad()
		{

		}

		protected override void _OnShow()
		{

		}

		protected override void _OnHide()
		{

		}

		protected override void _Dispose ()
		{

		}

		public void createOpportunityCell()
		{
//			var window = _window as UIEventLogWindow;
//			if (null != window && getVisible ())
//			{
//				window.buildOpportunityCell();
//			}

			kindStr = "buildOpportunityCell";
		}

		public void setBtnState(bool mstate)
		{
			m_bool_inner = mstate;
		}

		public bool m_bool_inner = false;

		public string kindStr = "";

		public override void Tick (float deltaTime)
		{
			var window = _window as UIEventLogWindow;
			if (null != window && getVisible ())
			{
				window.checkBottom();
			}
		}
	}
}
