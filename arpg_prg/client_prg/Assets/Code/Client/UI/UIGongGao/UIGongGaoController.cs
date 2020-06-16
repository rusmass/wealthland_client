using System;
using System.Collections.Generic;

namespace Client.UI
{
	public class UIGongGaoController:UIController<UIGongGaoWindow,UIGongGaoController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uigamegonggao.ab";
			}
		}

		public UIGongGaoController ()
		{
			//inforList=["11111","ssssssssssssss","skkkkkkkkkkkkk"};
//			inforList.Add("1111111111");
//			inforList.Add ("222222222");
//			inforList.Add ("55555555555");
		}


		public List<GonggaoVo> inforList = new List<GonggaoVo> ();
	}
}

