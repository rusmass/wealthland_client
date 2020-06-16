using System;

namespace Client.UI
{
    /// <summary>
    /// 把传入的数值转换成时间字符串
    /// </summary>
	public class HandleNumToTimeTool
	{
		public static string  ChangeNumberToTime(float value)
		{
			int timer = (int)((value % 3600));
			string timerStr;
			if (timer < 10)
			{
				timerStr = "0" + timer.ToString();
			}
			else
			{
				timerStr = timer.ToString();
			}
			return timerStr;
		}
	}
}

