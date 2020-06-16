using System;
using UnityEngine;

public static class ExtendedGameObject
{
	public static T GetComponentEx<T> (this GameObject go, string name)
	{
		if (null == go || null == go.transform || null == name) 
		{
			return default(T);
		}

		var transform = go.transform.DeepFindEx (name);

		if (null == transform) 
		{
			return default(T);
		}

		return transform.GetComponent<T> ();
	}

	public static void SetActiveEx(this GameObject go, bool active)
	{
		if (null != go) 
		{
			go.SetActive (active);
		}
	}

	public static GameObject CloneEx(this GameObject go, bool reserveName = false)
	{
		if (null != go) 
		{
			var cloned = GameObject.Instantiate (go);

			if (reserveName) 
			{
				cloned.name = go.name;
			}

			return cloned;
		}

		return null;
	}
}