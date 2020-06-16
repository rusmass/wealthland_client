using System;

/**
 * handler是一个回调，它可能间接引用了大量对象，及时置空;
 * */
public static class CallbackTools
{
	public static void Handle(ref Action handler, string title)
	{
		if (null != handler) 
		{
			try
			{
				handler();
			}
			catch(Exception ex)
			{
				Console.Error.WriteLine("CallBackTools.Handle() {0}, ex = {1}, \n\n StackTrace = {2}"
				                        , title, ex.ToString(), ex.StackTrace);
			}
			finally
			{
				handler = null;
			}
		}
	}

	public static void Handle<T>(ref Action<T> handler, T param, string title)
	{
		if (null != handler) 
		{
			try
			{
				handler(param);
			}
			catch(Exception ex)
			{
				Console.Error.WriteLine("CallBackTools.Handle() {0}, ex = {1}, \n\n StackTrace = {2}"
				                        , title, ex.ToString(), ex.StackTrace);
			}
			finally
			{
				handler = null;
			}
		}
	}
}