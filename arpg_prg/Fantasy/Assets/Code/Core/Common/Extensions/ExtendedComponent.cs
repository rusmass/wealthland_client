using System;
using UnityEngine;

public static class ExtendedComponent
{
	public static void SetActiveEx(this Component component, bool active)
	{
		if (null != component) 
		{
			component.gameObject.SetActiveEx (active);
		}
	}
}