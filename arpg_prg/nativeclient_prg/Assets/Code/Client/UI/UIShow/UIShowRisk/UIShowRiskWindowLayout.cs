using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIShowRiskWindow
	{
		class Layout
		{
			//bgimg
			public static string img_bg="image_bg";

			// top 
			public static string top="toptitle";
			public static string lb_cardname="lbcardname";

			//center
			public static string center="center";
			public static string loadImg="center/showImage";

			public static string img_wordimgLeft="center/wordbgleft";
			public static string scrollViewLeft="center/wordbgleft/ScrollView";
			public static string scrollViewPortLeft="center/wordbgleft/ScrollView/ViewPort";
			public static string scrollContentLeft="center/wordbgleft/ScrollView/ViewPort/Content";

			public static string lb_cardTitle="center/wordbgleft/ScrollView/ViewPort/Content/titletxt";
			public static string lb_desc="center/wordbgleft/ScrollView/ViewPort/Content/desctxt";	
			public static string lb_paymentName="center/wordbgleft/ScrollView/ViewPort/Content/paymentname";
			public static string lb_paymentTxt="center/wordbgleft/ScrollView/ViewPort/Content/paymentname/paymenttxt";



			public static string img_wordimg="center/wordbg";
			public static string scrollView="center/wordbg/ScrollView";
			public static string scrollViewPort="center/wordbg/ScrollView/ViewPort";
			public static string scrollContent="center/wordbg/ScrollView/ViewPort/Content";

			public static string lb_desc2="center/wordbg/ScrollView/ViewPort/Content/desctxt";		

			public static string lb_paymentName2="center/wordbg/ScrollView/ViewPort/Content/paymentname";
			public static string lb_paymentTxt2="center/wordbg/ScrollView/ViewPort/Content/paymentname/paymenttxt";

			public static string lb_timeName="center/wordbg/ScrollView/ViewPort/Content/paymentname/timescorename";
			public static string lb_timeTxt="center/wordbg/ScrollView/ViewPort/Content/paymentname/timescoretxt";

			public static string toggle_select="center/wordbg/ScrollView/ViewPort/Content/choicename/selecttoggle";


			public const string img_showImage = "showImage";
			public const string btn_sure = "surebtn";
		}

	}
}
