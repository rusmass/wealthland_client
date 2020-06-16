using System;

namespace Client.UI
{
	public partial class UIOtherCardWindow
	{
		class Layout
		{
            /// <summary>
            /// 20180619 关闭按钮,关闭展示按钮
            /// </summary>
            public static string btn_closeshow = "btncloseshow";
            //bgimg
            public static string img_bg="image_bg";

            /// <summary>
            /// 玩家动画transform
            /// </summary>
            public static string transform_card = "card";

			// top 
			public static string top="toptitle";
			public static string img_title="toptitle/img_kind";

			//center
			public static string center="center";
			public static string loadImg="center/showImage";
			public static string lbcardname="lbcardname";

			public static string img_wordimg="center/wordbg";
			public static string scrollView="center/wordbg/ScrollView";
			public static string scrollViewPort="center/wordbg/ScrollView/ViewPort";
			public static string scrollContent="center/wordbg/ScrollView/ViewPort/Content";

			public static string lb_desc="center/wordbg/ScrollView/ViewPort/Content/desctxt";

			public static string lb_desc2="center/wordbg/ScrollView/ViewPort/Content/desc2Name";
			public static string lb_desc3="center/wordbg/ScrollView/ViewPort/Content/desc3Name";

			//public static string img_salebg="salebgimg";

			//bottom
			public static string bottom="bottom";
			public static string btn_sure="bottom/surebtn";
			public static string btn_cancle="bottom/canclebtn";
			public static string btn_borrow="bottom/borrowbtn";

			public static string img_clock="clockbg";
			public static string lb_time="lb_time";

			//动作
			public static string cardAction = "cardAction";
			public static string cardAction1 = "cardAction1";

            #region 知识显示面板 
            /// <summary>
            /// 知识面板类型提示
            /// </summary>
            public static string lb_knowledgeHead = "lbknowledge";
            /// <summary>
            /// 知识面板的标题内容
            /// </summary>
            public static string lb_knowledgeTitle = "knowledgetitle";
            /// <summary>
            /// 知识面板的详情
            /// </summary>
            public static string lb_knowledgeContent = "knowledgedesc";
            /// <summary>
            /// 确定按钮
            /// </summary>
            public static string btn_knowledgeSure = "knowledgesurebtn";
            /// <summary>
            /// knowledge容器
            /// </summary>
            public static string obj_knowledge = "knowledge";
            #endregion
        }
	}
}

