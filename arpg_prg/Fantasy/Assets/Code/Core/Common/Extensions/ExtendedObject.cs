using UnityEngine;

public static class ExtendedObject
{
	public static void DestroyEx(this Object obj)
	{
		if (Application.isEditor) 
		{
			Object.DestroyImmediate (obj);
		}
		else
		{
			Object.Destroy (obj);
		}
	}
		
	public static Object InstantiateEx(this Object obj, Vector3 position, Quaternion rotation)
	{
		if (null != obj) 
		{
			var cloned = Object.Instantiate (obj, position, rotation);

			return cloned;
		}

		return null;
	}

	public static Object InstantiateEx(this Object obj)
	{
		if (null != obj) 
		{
			var cloned = Object.Instantiate (obj);
			return cloned;
		}

		return null;
	}
}