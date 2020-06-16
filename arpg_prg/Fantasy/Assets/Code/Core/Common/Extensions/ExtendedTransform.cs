using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtendedTransform
{
	public static Transform DeepFindEx (this GameObject father, string name)
	{
		if (null == father || null == father.transform) 
		{
			return null;
		}

		return father.transform.DeepFindEx (name);
	}

	public static void SetActiveEx(this Transform tf, bool active)
	{
		if (null != tf && null != tf.gameObject) 
		{
			tf.gameObject.SetActive (active);
		}
	}

	public static Transform DeepFindEx (this Transform father, string name)
	{
		if (null == father || null == name) 
		{
			return null;
		}

		if (father.name == name) 
		{
			return father;
		}

		_bfsQueue.Enqueue (father);
		while (_bfsQueue.Count > 0) 
		{
			father = _bfsQueue.Dequeue () as Transform;
			var childCount = father.childCount;
			if (childCount > 0) 
			{
				var targetChild = father.Find (name);
				if (null != targetChild) 
				{
					_bfsQueue.Clear ();
					return targetChild;
				}

				for (int i = 0; i < childCount; ++i) 
				{
					var child = father.GetChild (i);
					_bfsQueue.Enqueue (child);
				}
			}
		}

		return null;
	}

	public static Queue _bfsQueue = new Queue(32);
}