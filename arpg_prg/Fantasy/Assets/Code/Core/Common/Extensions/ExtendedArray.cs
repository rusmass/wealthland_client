
public static class ExtendedArray
{
	public static bool IsNullOrEmptyEx<T>(this T[] array)
	{
		return null == array || array.Length == 0;
	}
}	
