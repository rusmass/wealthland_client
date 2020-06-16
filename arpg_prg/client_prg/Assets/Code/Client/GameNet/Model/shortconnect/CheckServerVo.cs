using System;
using System.Collections.Generic;
using LitJson;

namespace GameNet
{
    /// <summary>
    /// 被屏蔽 ， 服务器的数据单元
    /// </summary>
	public class CheckServerBackVo
	{
		///0表示成功，1表示失败
		public int status;
		///:”提示信息”
		public string msg;

		public ServerListVo data; 

		/*data:[
			{“服务器一”:”电信”},
			{“服务器二”：”联通”}
		] */ 
	}

	public class ServerListVo
	{
		//LitJson.JsonMapper.to;
		//public List<LitJson.json> list;

	}

	public class ServerSingleData
	{
		public string serverName;
	}

	public class GetRecentServerVo
	{
		public string accountNumber;
	}
}

