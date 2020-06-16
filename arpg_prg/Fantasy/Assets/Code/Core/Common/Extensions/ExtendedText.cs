using System;
using UnityEngine.UI;

public static class ExtendedText
{
	public static void SetTextEx(this Text text, string value)
	{
		if (null != text) 
		{
			text.text = value;
		}
	}
}
