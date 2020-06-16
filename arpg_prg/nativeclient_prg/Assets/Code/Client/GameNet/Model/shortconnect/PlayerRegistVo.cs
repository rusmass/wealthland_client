using System;

namespace GameNet
{
    /// <summary>
    /// 发送注册手机账号时需要的数据
    /// </summary>
	public class PlayerRegistVo
	{
		//手机号码
		public string phone;
		//验证码
		public string code;
		//密码
		public string password;
	}
}

