using System;
using Core.Web;
using Core;

namespace Client.UI
{
	public abstract class UIControllerBase : Disposable
	{
		public UIControllerBase()
		{
			Inited = false;
		}

		protected sealed override void _DoDispose (bool isDisposing)
		{
			if (_visible) 
			{
				setVisible (false);
			}

			_UnloadUI ();
			_Dispose ();
			base._DoDispose (isDisposing);
		}

		public abstract IWebNode LoadUI(Action onCreated = null);
		public abstract void _UnloadUI();

		protected virtual void _OnLoad() {}
		protected virtual void _OnShow() {}
		protected virtual void _OnHide() {}
		protected virtual void _Dispose() {}
		public virtual void Tick(float deltaTime) {}

		protected virtual void _SyncWindowVisible() {}

		public bool getVisible()
		{
			return _visible;
		}

		public void setVisible(bool visible)
		{
			_visible = visible;
			if (_visible) 
			{
				_needSyncVisible = true;
                LoadUI();
                _OnShow ();
                _SyncWindowVisible();
            }
			else 
			{
				_needSyncVisible = true;
                _OnHide ();
				_SyncWindowVisible();
				_UnloadUI();
			}
        }

		internal bool _needSyncVisible;
		private bool _visible { get; set;}
		protected UIWindowBase _window;
		protected abstract string _windowResource { get;}

		public bool Inited;
	}
}

