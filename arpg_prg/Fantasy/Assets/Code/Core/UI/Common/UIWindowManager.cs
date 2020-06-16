using System;
using System.Collections;
using System.Collections.Generic;
using Client.UI;
using Core;
using Core.Web;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Client
{
	public class UIWindowManager
	{
		private UIWindowManager()
		{
			
		}

		public IWebNode CreateUIWindow<UIWindowT>(string resourcePath, Action<UIWindowT> onCreated = null)
			where UIWindowT : UIWindowBase, new()
		{
			return _LoadUI<UIWindowT> (resourcePath, window => {

				UIWindowT win = null;
				if (null != window)
				{
					if (_uiWindowsInstances.ContainsKey (window.Name)) {
						string log = string.Format("Add window to manager failed! resourceFileName = {0}", window.GetType().Name);
						Console.Error.WriteLine(log);
					}

					_uiWindowsInstances.Add(window.Name, window);

					win = window as UIWindowT;
				}

				if (null != onCreated)
				{
					onCreated(win);
				}
			});
		}


		public void DestroyUIWindow(ref UIWindowBase window)
		{
			if (null != window) 
			{
				var windowObject = _uiWindowsInstances [window.Name];
				_uiWindowsInstances.Remove (window.Name);
				windowObject.Dispose ();
				window = null;
			}
		}

		private IWebNode _LoadUI<UIWindowT>(string resourcePath, Action<UIWindowBase> onLoaded)
			where UIWindowT : UIWindowBase, new()
		{
			var argument = new WebArgument
			{
				localPath = resourcePath,
				flags = WebFlags.UnloadAllLoadedObjects
			};

			IWebNode ret = WebManager.Instance.LoadWebPrefab (argument, prefab => {
				using(prefab)
				{
					UIWindowBase window = null;
					Object scenceRoot = prefab.mainAsset.CloneEx(true);
					if (null != scenceRoot)
					{
						window = new UIWindowT();
						window.Initialise(scenceRoot as GameObject);
					}

					if (null != onLoaded)
					{
						onLoaded(window);
					}
				}
			});

			return ret;
		}

		public static UIWindowManager Instance = new UIWindowManager();
		private Dictionary<string, UIWindowBase> _uiWindowsInstances = new Dictionary<string, UIWindowBase>();
	}
}

