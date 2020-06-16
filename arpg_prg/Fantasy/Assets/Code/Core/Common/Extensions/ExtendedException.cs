using System;

public static class ExtendedException
{
	public static string ToStringEx(this Exception ex)
	{
		if (null != ex) 
		{
			return string.Concat("[", ex.ToString(), "\n\n", ex.StackTrace, "]");
		}

		return string.Empty;
	}
}