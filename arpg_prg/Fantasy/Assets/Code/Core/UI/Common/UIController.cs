using System;
using Core.Web;

namespace Client.UI
{
	public abstract class UIController<WindowChild, ControllerChild> : UIControllerBase
		where WindowChild : UIWindow<WindowChild, ControllerChild>, new()
		where ControllerChild : UIController<WindowChild, ControllerChild>, new()
	{
		public UIController()
		{
			
		}

		protected override string _windowResource 
		{
			get { throw new NotImplementedException (); }
		}

		public sealed override void _UnloadUI ()
		{
			if (null != _window) 
			{
				var winBase = _window as UIWindowBase;
				UIWindowManager.Instance.DestroyUIWindow (ref winBase);
				_window = null;
				Inited = false;
			}

			if (null != _loadingWebNode) 
			{
				_loadingWebNode.kill ();
				_loadingWebNode = null;
			}
		}

		public sealed override IWebNode LoadUI (Action onCreated = null)
		{
			if (Inited) 
			{
				return EmptyWebNode.Instance;
			}

			if (null != _loadingWebNode) 
			{
				return _loadingWebNode;
			}

			_loadingWebNode = UIWindowManager.Instance.CreateUIWindow<WindowChild> (_windowResource, window => 
			{
				_window = window;
				Inited = true;
				_OnLoad();

				_SyncWindowVisible();

				CallbackTools.Handle(ref onCreated, "[UIController.LoadUI()]");
			});

			return _loadingWebNode;
		}

		protected sealed override void _SyncWindowVisible()
		{
			if (null != _window && _needSyncVisible) 
			{
				_needSyncVisible = false;
				_window.Visible = getVisible();
			}
		}

		private IWebNode _loadingWebNode = null;
	}
}

