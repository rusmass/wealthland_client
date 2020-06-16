using System;
using Client.UI;
using System.Collections;
using System.Collections.Generic;
using Core.Web;

namespace Client
{
	public class UIControllerManager
	{
		private UIControllerManager()
		{
			
		}

		public IWebNode LoadUI<UIControllerT>(Action<UIControllerT> onCreated = null)
			where UIControllerT : UIControllerBase, new()
		{
			UIControllerT controller = GetController<UIControllerT> ();
			if (null != controller) 
			{
				return controller.LoadUI (() => CallbackTools.Handle (ref onCreated, controller, "[UIControllerManager.LoadUI()]"));
			}

			return EmptyWebNode.Instance;
		}

		public UIControllerT GetController<UIControllerT>()
			where UIControllerT : UIControllerBase, new()
		{
			var type = typeof(UIControllerT);

			UIControllerBase controller;
			if (_controllers.TryGetValue (type, out controller)) 
			{
				return controller as UIControllerT;
			}

			var control = new UIControllerT ();
			_controllers [type] = control;
			return control;
		}

		public void DestroyController(UIControllerBase controller)
		{
			if (null != controller) 
			{
				_controllers.Remove (controller.GetType ());

				try
				{
					controller.Dispose();
				}
				catch(Exception e) 
				{
					Console.Error.WriteLine (e.Message);
					Console.Error.WriteLine (e.StackTrace);
				}
			}
		}

		public void DestroyController<UIControllerT>()
			where UIControllerT : UIControllerBase, new()
		{
			var type = typeof(UIControllerT);
			UIControllerBase controller;
			if (_controllers.TryGetValue (type, out controller)) 
			{
				DestroyController (controller);
			}
		}

		public void Tick(float deltaTime)
		{
			var e = _controllers.GetEnumerator ();
			while (e.MoveNext ()) 
			{
				var control = e.Current.Value;
				if (control.Inited) 
				{
					control.Tick (deltaTime);
				}
			}
		}

		private Dictionary<Type, UIControllerBase> _controllers = new Dictionary<Type, UIControllerBase>();
		public static UIControllerManager Instance = new UIControllerManager();
	}
}

