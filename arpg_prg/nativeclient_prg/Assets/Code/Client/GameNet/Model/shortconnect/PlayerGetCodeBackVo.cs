using System;

namespace GameNet
{
    /// <summary>
    /// 接收获取验证码返回的数据
    /// </summary>
    public class PlayerGetCodeBackVo
	{
		/// <summary>
		/// The status.//0成功，1失败
		/// </summary>
		public int status;   
		public string msg;

		/// <summary>
		/// The data.服务器返回的验证码
		/// </summary>
		public string code="";
	}
}

