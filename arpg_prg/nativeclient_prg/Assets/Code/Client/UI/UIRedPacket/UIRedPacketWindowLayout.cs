using System;

namespace Client.UI
{
	public partial class UIRedPacketWindow
	{
		class Layout
		{
            public static string getpacketpanel = "getpacketpanel";
            public static string sendpacketpanel = "sendpacketpanel";
            public static string packpacketpanel = "packpacketpanel";
            
            //收红包
            public static string obj_redpacket = "redpacket";
            public static string obj_notgetpacket = "notgetpacket";
            public static string obj_getpacket = "getpacket";
            public static string btn_open = "/btn_open";
            public static string lb_timerecord = "lb_timerecord";
            public static string lb_getCashCount = "/lb_getCashCount";
            public static string lb_getprompt = "/lb_getprompt";
            //发红包
            public static string btn_send = "sendredpacket/btn_send";
            public static string input_cash = "sendredpacket/InputField";

			public static string item_clock="clockitem";

            //包红包
            public static string btn_pack = "packredpacket/btn_pack";
            public static string lb_packprompt = "packredpacket/lb_packprompt";
			public static string btn_cancle="packredpacket/btn_cancle";
        }
	}
}

