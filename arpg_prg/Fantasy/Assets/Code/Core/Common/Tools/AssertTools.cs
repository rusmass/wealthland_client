
namespace Core
{
	public static class AssertTools
	{
		public static void IsNotNull<T> (T obj)
		{
			if (null == obj) 
			{
				var message = string.Format("obj is null, obj type = {1}", typeof(T));
				Console.Error.WriteLine(message);
			}
		}
	}
}

