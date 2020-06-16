using System;
using UnityEngine;

namespace Client
{
    /// <summary>
    /// 把不同的参数，转为金币数目
    /// </summary>
	public class HandleStringTool
	{
		
		public static string HandleMoneyTostring(int value)
		{
			var tmpStr = value.ToString();
			if (value >= 100000)
			{
				var tmpMoney =(int) (value / 10000f);
				tmpStr = string.Format ("{0}万",tmpMoney.ToString());
			}
			return tmpStr;
		}


		public static string HandleMoneyTostring(float value)
		{
			var tmpValue=Mathf.RoundToInt(value);
			var tmpStr =HandleMoneyTostring(tmpValue);
			return tmpStr;
		}

		public static string HandleMoneyTostring(string value)
		{
			var tmpValue = float.Parse (value);

			var tmpStr=HandleMoneyTostring(tmpValue);

			return tmpStr;

		}
	}
}

