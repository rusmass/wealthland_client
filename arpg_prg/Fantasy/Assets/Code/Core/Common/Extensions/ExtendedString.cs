
public static class ExtendedString
{
	public static bool StartWithAnyEx(this string text, string[] candidates)
	{
		if (null != text && null != candidates) 
		{
			for (int i = 0; i < candidates.Length; ++i)
			{
				if (text.StartsWith(candidates[i]))
				{
					return true;
				}
			}
		}

		return false;
	}

	public static bool EndWithAnyEx(this string text, string[] candidates)
	{
		if (null != text && null != candidates) 
		{
			for (int i = 0; i < candidates.Length; ++i)
			{
				if (text.EndsWith(candidates[i]))
				{
					return true;
				}
			}
		}

		return false;
	}
}