using System;
using Client;
using UnityEngine;

namespace Client.UI
{
	public abstract class UIWindow<WindowChild, ControllerChild> : UIWindowBase 
		where WindowChild : UIWindow<WindowChild, ControllerChild>, new()
		where ControllerChild : UIController<WindowChild, ControllerChild>, new()
	{
		public sealed override void Initialise (GameObject go)
		{
			_controller = UIControllerManager.Instance.GetController<ControllerChild> ();
			base.Initialise (go);
		}

		protected ControllerChild _controller;
	}
}

